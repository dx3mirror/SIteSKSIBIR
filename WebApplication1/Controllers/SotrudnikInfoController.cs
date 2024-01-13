using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Factory;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Strategy;
using WebApplication1.UnitOfWork;
using WebApplication2;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class SotrudnikInfoController : Controller
    {
        private readonly KadrovikContext _kadrovikContext;
        private readonly IFindSotrudnikUnitOfWork _findunitOfWork;
        private readonly IFileConversionStrategy _conversionStrategy;
        private readonly IAddSotrudnikRepository _addsotrudnikRepository;
        private readonly IAddSotrudnikUnitOfWork _addunitOfWork;
        private readonly ISotrudnikUpdateRepository _sotrudnikUpdateRepository;
        private readonly ISotrudnikUpdateUnitOfWork _sotrudnikUpdateUnitOfWork;
        private readonly IDoljnostUnitOfWork _doljnostunitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IUpdateDoljnostUnitOfWork _updatedoljsnost;
        private readonly ILogger<SotrudnikInfoController> _logger;
        private readonly IAddDoljnostUnitOfWork _adddoljnostunitOfWork;
        private readonly IAddDoljstostRepository _adddoljnostRepository;
        private readonly IDoljnostFactory _doljnostFactory;
        private readonly IViewOtpuskUnitOfWork _viewOtpuskUnitOfWork;
        private readonly IOtpuskViewModelFactory _otpuskViewModelFactory;
        public SotrudnikInfoController(
            IFindSotrudnikUnitOfWork findunitofwork,
            IFileConversionStrategy conversionStrategy,
            IAddSotrudnikRepository addsotrudnikRepository,
            IAddSotrudnikUnitOfWork addunitOfWork,
            ISotrudnikUpdateRepository sotrudnikUpdateRepository,
            ISotrudnikUpdateUnitOfWork sotrudnikUpdateUnitOfWork,
            IDoljnostUnitOfWork doljnostunitOfWork,
            IUpdateDoljnostUnitOfWork updatedoljsnost,
            ILogger<SotrudnikInfoController> logger,
            IConfiguration configuration,
            KadrovikContext kadrovikContext,
            IAddDoljnostUnitOfWork adddoljnostunitOfWork,
            IAddDoljstostRepository adddoljnostRepository,
            IDoljnostFactory doljnostFactory,
            IViewOtpuskUnitOfWork viewOtpuskUnitOfWork,
            IOtpuskViewModelFactory otpuskViewModelFactory
            )
        {
            _addsotrudnikRepository = addsotrudnikRepository;
            _addunitOfWork = addunitOfWork;
            _conversionStrategy = conversionStrategy;
            _findunitOfWork = findunitofwork;
            _sotrudnikUpdateRepository = sotrudnikUpdateRepository;
            _sotrudnikUpdateUnitOfWork = sotrudnikUpdateUnitOfWork;
            _doljnostunitOfWork = doljnostunitOfWork;
            _updatedoljsnost = updatedoljsnost;
            _configuration = configuration;
            _logger = logger;
            _kadrovikContext = kadrovikContext;
            _adddoljnostunitOfWork = adddoljnostunitOfWork;
            _adddoljnostRepository = adddoljnostRepository;
            _doljnostFactory = doljnostFactory;
            _viewOtpuskUnitOfWork = viewOtpuskUnitOfWork;
            _otpuskViewModelFactory = otpuskViewModelFactory;
        }


        public IActionResult AddSotrudnik() => View();

        public async Task<IActionResult> Doljnost()
        {
            int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

            if (sotrudnikId.HasValue)
            {
                var doljnostList = await GetDoljnostiForSotrudnikAsync(sotrudnikId.Value);
                var viewModel = CreateDoljnostViewModel(sotrudnikId.Value, doljnostList);

                return View(viewModel);
            }

            return RedirectToAction("Index", "SotrudnikInfo");
        }

        private async Task<List<Doljnost>> GetDoljnostiForSotrudnikAsync(int sotrudnikId)
        {
            return await _doljnostunitOfWork.DoljnostRepository.GetDoljnostiForSotrudnikAsync(sotrudnikId);
        }

        private DoljnostViewModel CreateDoljnostViewModel(int sotrudnikId, List<Doljnost> doljnostList)
        {
            var lastName = HttpContext.Session.GetString("LastName") ?? "";
            var firstName = HttpContext.Session.GetString("FirstName") ?? "";
            var patranomic = HttpContext.Session.GetString("Patranomic") ?? "";

            return new DoljnostViewModel
            {
                SotrudnikId = sotrudnikId,
                LastName = lastName,
                FirstName = firstName,
                Patranomic = patranomic,
                DoljnostList = doljnostList
            };
        }


        [HttpPost("/api/UpdateDoljnost")]
        public async Task<IActionResult> UpdateDoljnost([FromBody] Doljnost doljnostData)
        {
            try
            {
                if (!IsValidDoljnostData(doljnostData))
                {
                    return BadRequest(new { success = false, error = "Некорректные данные для обновления." });
                }

                int doljnostId = doljnostData.Id;
                var existingDoljnost = await GetExistingDoljnostAsync(doljnostId);

                if (existingDoljnost == null)
                {
                    return BadRequest(new { success = false, error = "Запись не найдена." });
                }

                var updatedDoljnost = _doljnostFactory.CreateDoljnostForUpdate(
                    doljnostId,
                    doljnostData.NaimenoviyDoljnosti ?? throw new ArgumentNullException(nameof(doljnostData.NaimenoviyDoljnosti)),
                    doljnostData.SKogo ?? throw new ArgumentNullException(nameof(doljnostData.SKogo)),
                    doljnostData.PoKokoe ?? throw new ArgumentNullException(nameof(doljnostData.PoKokoe)),
                    doljnostData.KolVo ?? 0,
                    doljnostData.Otvetstveniy ?? throw new ArgumentNullException(nameof(doljnostData.Otvetstveniy))
                );

                UpdateExistingDoljnost(existingDoljnost, updatedDoljnost);
                await _updatedoljsnost.SaveAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении должности.");
                return Json(new { success = false, error = ex.Message });
            }
        }

        private bool IsValidDoljnostData(Doljnost doljnostData)
        {
            return doljnostData != null && doljnostData.Id > 0 && !string.IsNullOrWhiteSpace(doljnostData.NaimenoviyDoljnosti);
        }

        private async Task<Doljnost> GetExistingDoljnostAsync(int doljnostId)
        {
            return await _updatedoljsnost.DoljnostRepository.GetByIdAsync(doljnostId);
        }

        private void UpdateExistingDoljnost(Doljnost existingDoljnost, Doljnost updatedDoljnost)
        {
            existingDoljnost.NaimenoviyDoljnosti = updatedDoljnost.NaimenoviyDoljnosti;
            existingDoljnost.SKogo = updatedDoljnost.SKogo;
            existingDoljnost.PoKokoe = updatedDoljnost.PoKokoe;
            existingDoljnost.KolVo = updatedDoljnost.KolVo;
            existingDoljnost.Otvetstveniy = updatedDoljnost.Otvetstveniy;
        }


        [HttpPost("/api/AddDoljnost")]
        public IActionResult AddDoljnost([FromBody] Doljnost doljnostData)
        {
            try
            {
                int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

                if (!IsValidDoljnostDataForAdd(doljnostData))
                {
                    return BadRequest(new { success = false, error = "Некорректные данные для добавления должности." });
                }

                var doljnost = CreateDoljnostForAdd(sotrudnikId, doljnostData);

                _adddoljnostRepository.Add(doljnost);
                _adddoljnostunitOfWork.SaveChanges();

                return Ok(new { success = true });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { success = false, error = "Отсутствуют обязательные данные для должности." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        private bool IsValidDoljnostDataForAdd(Doljnost doljnostData)
        {
            return doljnostData != null
                && !string.IsNullOrWhiteSpace(doljnostData.NaimenoviyDoljnosti)
                && !string.IsNullOrWhiteSpace(doljnostData.SKogo)
                && !string.IsNullOrWhiteSpace(doljnostData.PoKokoe)
                && doljnostData.Otvetstveniy != null; 
        }

        private Doljnost CreateDoljnostForAdd(int? sotrudnikId, Doljnost doljnostData)
        {
            return _doljnostFactory.CreateDoljnostForAdd(
                sotrudnikId,
                doljnostData.NaimenoviyDoljnosti ?? throw new ArgumentNullException(nameof(doljnostData.NaimenoviyDoljnosti)),
                doljnostData.SKogo ?? throw new ArgumentNullException(nameof(doljnostData.SKogo)),
                doljnostData.PoKokoe ?? throw new ArgumentNullException(nameof(doljnostData.PoKokoe)),
                doljnostData.KolVo ?? 0,
                doljnostData.Otvetstveniy ?? throw new ArgumentNullException(nameof(doljnostData.Otvetstveniy))
            );
        }

        public async Task<IActionResult> Otpusk()
        {
            int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

            if (sotrudnikId.HasValue)
            {
                var viewModel = await _otpuskViewModelFactory.CreateOtpuskViewModelAsync(sotrudnikId.Value);

                return View(viewModel);
            }

            return RedirectToAction("Index", "SotrudnikInfo");
        }

        [HttpPost("/api/UpdateOtpusk")]
        public IActionResult UpdateOtpusk([FromBody] Otpusk otpuskData)
        {
            try
            {
                int otpuskId = otpuskData.Id;

                var existingOtpusk = _kadrovikContext.Otpusks.Find(otpuskId);

                if (existingOtpusk == null)
                {
                    return BadRequest(new { success = false, error = "Запись не найдена." });
                }

                existingOtpusk.VidOtpuska = otpuskData.VidOtpuska;
                existingOtpusk.PeriodS = otpuskData.PeriodS;
                existingOtpusk.PeriodPo = otpuskData.PeriodPo;
                existingOtpusk.Day = otpuskData.Day;
                existingOtpusk.SKakogo = otpuskData.SKakogo;
                existingOtpusk.PoKakoe = otpuskData.PoKakoe;
                existingOtpusk.Prichina = otpuskData.Prichina;

                _kadrovikContext.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        [HttpPost("/api/AddOtpusk")]
        public IActionResult AddOtpusk([FromBody] Otpusk otpuskData)
        {
            try
            {
                int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

                if (otpuskData == null)
                {
                    return Json(new { success = false, error = "Отсутствуют данные отпуска." });
                }

                var otpusk = new Otpusk
                {
                    FkSotrudnik = sotrudnikId,
                    VidOtpuska = otpuskData.VidOtpuska,
                    PeriodS = otpuskData.PeriodS,
                    PeriodPo = otpuskData.PeriodPo,
                    Day = otpuskData.Day,
                    SKakogo = otpuskData.SKakogo,
                    PoKakoe = otpuskData.PoKakoe,
                    Prichina = otpuskData.Prichina
                };

                _kadrovikContext.Otpusks.Add(otpusk);
                _kadrovikContext.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        public IActionResult Family()
        {
            int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

            if (sotrudnikId.HasValue)
            {
                var familyList = _kadrovikContext.Families
                    .Where(f => f.FkSotrudnik == sotrudnikId)
                    .ToList();

                var lastName = HttpContext.Session.GetString("LastName") ?? "";
                var firstName = HttpContext.Session.GetString("FirstName") ?? "";
                var patranomic = HttpContext.Session.GetString("Patranomic") ?? "";

                // Создаем модель представления
                var viewModel = new FamilyViwModel
                {
                    SotrudnikId = sotrudnikId.Value,
                    LastName = lastName,
                    FirstName = firstName,
                    Patranomic = patranomic,
                    FamilyeList = familyList
                };

                return View(viewModel);
            }

            return RedirectToAction("Index", "SotrudnikInfo");
        }
        [HttpPost("/api/UpdateFamily")]
        public IActionResult UpdateFamily([FromBody] Family familyData)
        {
            try
            {
                int familyId = familyData.Id;

                var existingFamilyMember = _kadrovikContext.Families.Find(familyId);

                if (existingFamilyMember == null)
                {
                    return Json(new { success = false });
                }

                existingFamilyMember.Fio = familyData.Fio;
                existingFamilyMember.StepenRodstva = familyData.StepenRodstva;
                existingFamilyMember.DataRojdeniya = familyData.DataRojdeniya;

                _kadrovikContext.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpPost("/api/AddFamilyMember")]
        public IActionResult AddFamilyMember([FromBody] Family familyData)
        {
            try
            {
                int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

                var familyMember = new Family
                {
                    FkSotrudnik = sotrudnikId,
                    Fio = familyData.Fio,
                    StepenRodstva = familyData.StepenRodstva,
                    DataRojdeniya = familyData.DataRojdeniya
                };

                _kadrovikContext.Families.Add(familyMember);
                _kadrovikContext.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpPost("/api/UpdatePassport")]
        public IActionResult UpdatePassport([FromBody] Passport passportData)
        {
            try
            {
                int passportId = passportData.Id;

                var existingPassport = _kadrovikContext.Passports.Find(passportId);

                if (existingPassport == null)
                {
                    return BadRequest(new { success = false, error = "Запись не найдена." });
                }

                existingPassport.NomerPasporta = passportData.NomerPasporta;
                existingPassport.Datavidachi = passportData.Datavidachi;
                existingPassport.DataregistrachiiPoMestu = passportData.DataregistrachiiPoMestu;
                existingPassport.Vidan = passportData.Vidan;
                existingPassport.AdressjitelstvaPopasportu = passportData.AdressjitelstvaPopasportu;
                existingPassport.AressFakticheskiy = passportData.AressFakticheskiy;

                _kadrovikContext.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpPost("/api/AddPassport")]
        public IActionResult AddPassport([FromBody] Passport passportData)
        {
            try
            {
                int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

                var passport = new Passport
                {
                    FkSotrudnik = sotrudnikId,
                    NomerPasporta = passportData.NomerPasporta,
                    Datavidachi = passportData.Datavidachi,
                    DataregistrachiiPoMestu = passportData.DataregistrachiiPoMestu,
                    Vidan = passportData.Vidan,
                    AdressjitelstvaPopasportu = passportData.AdressjitelstvaPopasportu,
                    AressFakticheskiy = passportData.AressFakticheskiy
                };

                _kadrovikContext.Passports.Add(passport);
                _kadrovikContext.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public IActionResult Passport()
        {
            int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

            if (sotrudnikId.HasValue)
            {
                var passportList = _kadrovikContext.Passports
                    .Where(p => p.FkSotrudnik == sotrudnikId)
                    .ToList();

                var lastName = HttpContext.Session.GetString("LastName") ?? "";
                var firstName = HttpContext.Session.GetString("FirstName") ?? "";
                var patranomic = HttpContext.Session.GetString("Patranomic") ?? "";

                // Создаем модель представления
                var viewModel = new PassportViewModel
                {
                    SotrudnikId = sotrudnikId.Value,
                    LastName = lastName,
                    FirstName = firstName,
                    Patranomic = patranomic,
                    PassporteList = passportList
                };

                return View(viewModel);
            }
            return RedirectToAction("Index", "SotrudnikInfo");
        }
        public IActionResult Obrozovaniy()
        {
            int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

            if (sotrudnikId.HasValue)
            {
                
                var obrazovanieList = _kadrovikContext.Obrazovanies
                    .Where(o => o.FkSotrudnik == sotrudnikId)
                    .ToList();

                var lastName = HttpContext.Session.GetString("LastName") ?? "";
                var firstName = HttpContext.Session.GetString("FirstName") ?? "";
                var patranomic = HttpContext.Session.GetString("Patranomic") ?? "";

                // Создаем модель представления
                var viewModel = new SotrudnikViewModel
                {
                    SotrudnikId = sotrudnikId.Value,
                    LastName = lastName,
                    FirstName = firstName,
                    Patranomic = patranomic,
                    ObrazovanieList = obrazovanieList
                };

                return View(viewModel);
            }
            return RedirectToAction("Index", "SotrudnikInfo");
        }
        [HttpPost("/api/UpdateObrazovanie")]
        public IActionResult UpdateObrazovanie([FromBody] Obrazovanie obrazovanieData)
        {
            try
            {
                int obrazovanieId = obrazovanieData.Id;

                var existingObrazovanie = _kadrovikContext.Obrazovanies.Find(obrazovanieId);

                if (existingObrazovanie == null)
                {
                    return BadRequest(new { success = false, error = "Запись не найдена." });
                }

                existingObrazovanie.Obrazovanie1 = obrazovanieData.Obrazovanie1;
                existingObrazovanie.Nazvanieuchrejdeniya = obrazovanieData.Nazvanieuchrejdeniya;
                existingObrazovanie.KvalifikachiyapoObrazovaniyu = obrazovanieData.KvalifikachiyapoObrazovaniyu;
                existingObrazovanie.Nazvanieuchrejdeniya2 = obrazovanieData.Nazvanieuchrejdeniya2;
                existingObrazovanie.KvalifikachiyapoObrazovaniyu2 = obrazovanieData.KvalifikachiyapoObrazovaniyu2;
                existingObrazovanie.Poslevuzovoe = obrazovanieData.Poslevuzovoe;
                existingObrazovanie.ProfessiaOsnova = obrazovanieData.ProfessiaOsnova;
                existingObrazovanie.ProfessiaDopolnitel = obrazovanieData.ProfessiaDopolnitel;

                _kadrovikContext.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("/api/AddObrozovaniySotrudnik")]
        public IActionResult AddObrazovanie([FromBody] Obrazovanie obrazovanieData)
        {
            try
            {
                int? sotrudnikId = HttpContext.Session.GetInt32("SotrudnikId");

                var obrazovanie = new Obrazovanie
                {
                    FkSotrudnik = sotrudnikId,
                    Obrazovanie1 = obrazovanieData.Obrazovanie1,
                    Nazvanieuchrejdeniya = obrazovanieData.Nazvanieuchrejdeniya,
                    KvalifikachiyapoObrazovaniyu = obrazovanieData.KvalifikachiyapoObrazovaniyu,
                    Nazvanieuchrejdeniya2 = obrazovanieData.Nazvanieuchrejdeniya2,
                    KvalifikachiyapoObrazovaniyu2 = obrazovanieData.KvalifikachiyapoObrazovaniyu2,
                    Poslevuzovoe = obrazovanieData.Poslevuzovoe,
                    ProfessiaOsnova = obrazovanieData.ProfessiaOsnova,
                    ProfessiaDopolnitel = obrazovanieData.ProfessiaDopolnitel
                };

                _kadrovikContext.Obrazovanies.Add(obrazovanie);

                _kadrovikContext.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }



        public async Task<IActionResult> Index()
        {
            var sotrudniki = await _findunitOfWork.SotrudnikRepository.FindSotrudniksAsync();
            return View(sotrudniki);
        }
        [HttpGet("/api/Find")]
        public IActionResult SearchSotrudniks(string searchText)
        {
            var sotrudniki = _kadrovikContext.Sotrudniks
                .Where(s =>
                    EF.Functions.Like(s.Lastname, $"%{searchText}%") ||
                    EF.Functions.Like(s.Firstname, $"%{searchText}%") ||
                    EF.Functions.Like(s.Patranomic, $"%{searchText}%") ||
                    EF.Functions.Like(s.Adress, $"%{searchText}%") ||
                    EF.Functions.Like(s.MestoRojd, $"%{searchText}%") ||
                    EF.Functions.Like(s.InYz, $"%{searchText}%") ||
                    EF.Functions.Like(s.Brak, $"%{searchText}%") ||
                    EF.Functions.Like(s.IdentityNomer, $"%{searchText}%") ||
                    EF.Functions.Like(s.Okin, $"%{searchText}%"))
                .ToList();
            return View(sotrudniki);
        }

        [HttpPost("/api/DateSingle")]
        public IActionResult Single([FromBody] SotrudnikDate sotrudnikData)
        {
            _logger.LogInformation("Received data: {@data}", sotrudnikData.ID);
            HttpContext.Session.SetInt32("SotrudnikId", sotrudnikData.ID);
            HttpContext.Session.SetString("LastName", sotrudnikData.LastName ?? "");
            HttpContext.Session.SetString("FirstName", sotrudnikData.FirstName ?? "");
            HttpContext.Session.SetString("Patranomic", sotrudnikData.Patranomic ?? "");

            return Json(new { success = true });
        }
        [HttpPost("/api/UpdateSotrudnik")]
        public async Task<IActionResult> UpdateSotrudnikWithPhoto([FromForm] Sotrudnik model, IFormFile photoChange)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                var existingSotrudnik = await _sotrudnikUpdateRepository.FindSotrudnikByIdAsync(model.Id);

                if (existingSotrudnik == null)
                {
                    return BadRequest("Sotrudnik not found");
                }

                _sotrudnikUpdateRepository.UpdateSotrudnik(existingSotrudnik, model);
                existingSotrudnik.Del = "no";

                if (photoChange != null && photoChange.Length > 0)
                {
                    existingSotrudnik.Avatar = await _conversionStrategy.ConvertFileToByteArrayAsync(photoChange);
                }

                await _sotrudnikUpdateUnitOfWork.SaveChangesAsync();

                return Ok(new { success = true, id = existingSotrudnik.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("/api/AddSotrudnik")]
        public async Task<IActionResult> AddSotrudnikWithPhoto([FromForm] Sotrudnik model, IFormFile photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _addsotrudnikRepository?.AddSotrudnikAsync(model);
                    await _addunitOfWork.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("Invalid model state");
                }

                if (photo != null && photo.Length > 0)
                {
                    model.Avatar = await _conversionStrategy.ConvertFileToByteArrayAsync(photo);
                    await _addunitOfWork.SaveChangesAsync();
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
                        /* [HttpGet]
                 [Route("GenerateReport")]
                 public IActionResult GenerateReport()
                 {
                     string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                
                
                     string resourcePath = Path.Combine(wwwRootPath, "Excel", "T2_Form.xlsx");
                
                     // Создание временного файла и запись в него ресурса
                     string tempPath = Path.GetTempFileName();
                     System.IO.File.WriteAllBytes(tempPath, System.IO.File.ReadAllBytes(resourcePath));
                     // Ваш существующий код
                     Excel.Application excelApplication = new Excel.Application();
                     Excel._Workbook excelWorkbook;
                     excelWorkbook = excelApplication.Workbooks.Add(); // Используем Add для создания нового Excel-документа
                     Excel.Worksheet worksheet = excelWorkbook.Sheets[1];
                     Excel.Range r1 = worksheet.Range[worksheet.Cells[24, 2], worksheet.Cells[26, 2]];
                
                     string sqlText = $@"SELECT                      
                                             [ID],
                                             [lastname],
                                             [nam],
                                             [patronomic],
                                             [telefon],
                                             [mesto_rojd],
                                             [Pol],
                                             [Identitynomer],
                                             [OKIN],
                                             [nazvanieuchrejdeniya],
                                             [kvalifikachiyapoObrazovaniyu],
                                             [nazvanieuchrejdeniya2],
                                             [kvalifikachiyapoObrazovaniyu2],
                                             [poslevuzovoe],
                                             [adress],
                                             [datarojdeniya], 
                                             [in_yaz],
                                             [grajdanstvo],
                                             [brak],
                                             [naimenoviy_doljnosti],
                                             [obrazovanie]
                                         FROM Sotrudnik, doljnost, Obrazovanie
                                         WHERE Sotrudnik.ID = Obrazovanie.fk_kod_sotrudnika AND Sotrudnik.ID = doljnost.fk_kod_sotrudnik AND Sotrudnik.ID = @currValue";
                
                     string connectionString = _configuration.GetConnectionString("DefaultConnection");
                     SqlConnection conn = new SqlConnection(connectionString);
                
                     using (SqlCommand cmd = new SqlCommand(sqlText, conn))
                     {
                         cmd.Parameters.AddWithValue("@currValue", HttpContext.Session.GetInt32("SotrudnikId"));
                         conn.Open();
                         SqlDataReader dr = cmd.ExecuteReader();
                
                         if (dr.HasRows)
                         {
                             while (dr.Read())
                             {
                                 worksheet.Cells[18, 2] = dr["lastname"];
                                 worksheet.Cells[8, 3] = dr["Identitynomer"];
                                 worksheet.Cells[18, 5] = dr["nam"];
                                 worksheet.Cells[18, 8] = dr["patronomic"];
                                 worksheet.Cells[15, 9] = dr["telefon"];
                                 worksheet.Cells[21, 3] = dr["datarojdeniya"];
                                 worksheet.Cells[25, 4] = dr["in_yaz"];
                                 worksheet.Cells[24, 3] = dr["grajdanstvo"];
                                 worksheet.Cells[8, 5] = dr["brak"];
                                 worksheet.Cells[29, 3] = dr["obrazovanie"];
                                 worksheet.Cells[56, 3] = dr["naimenoviy_doljnosti"];
                                 worksheet.Cells[23, 3] = dr["mesto_rojd"];
                                 worksheet.Cells[8, 10] = dr["OKIN"];
                                 worksheet.Cells[24, 9] = dr["OKIN"];
                                 worksheet.Cells[25, 9] = dr["OKIN"];
                                 worksheet.Cells[27, 9] = dr["OKIN"];
                                 worksheet.Cells[29, 9] = dr["OKIN"];
                                 worksheet.Cells[46, 9] = dr["OKIN"];
                                 worksheet.Cells[33, 1] = dr["nazvanieuchrejdeniya"];
                                 worksheet.Cells[36, 1] = dr["kvalifikachiyapoObrazovaniyu"];
                                 worksheet.Cells[40, 1] = dr["nazvanieuchrejdeniya2"];
                                 worksheet.Cells[43, 1] = dr["kvalifikachiyapoObrazovaniyu2"];
                                 worksheet.Cells[46, 5] = dr["poslevuzovoe"];
                             }
                         }
                     }
                
                     // Сохранение документа во временный файл
                     string tempPath = System.IO.Path.GetTempFileName();
                     excelWorkbook.SaveAs(tempPath);
                     excelWorkbook.Close();
                     excelApplication.Quit();
                
                     // Освобождение ресурсов Excel
                     System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApplication);
                     System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
                     System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                     System.Runtime.InteropServices.Marshal.ReleaseComObject(r1);
                
                     // Возвращаем Excel-документ в ответе
                     return File(tempPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
                 }*/

    }


}
