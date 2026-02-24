
using EscolaApp.Repositorio;
using escolakell.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscolaApp.Controllers
{
    public class DiarioController : Controller
    {
        private readonly DiarioRepositorio _repository;
        private readonly ILogger<DiarioController> _logger;

        public DiarioController(IConfiguration configuration, ILogger<DiarioController> logger)
        {
            // PASSA o logger para o repositório
            _logger = logger;
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var repoLogger = loggerFactory.CreateLogger<DiarioRepositorio>();
            _repository = new DiarioRepositorio(configuration, repoLogger); // USA O CONSTRUTOR COM LOGGER!
        }

       
        [HttpGet]
        public IActionResult Index()
        {
            var alunos = _repository.ListarTodosComAluno();
            return View(alunos);
        }
     
        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var aluno = _repository.ObterPorId(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

       
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public IActionResult ExcluirConfirmado(int id)
        {
            _repository.Excluir(id);
            TempData["Mensagem"] = "Aluno excluído com sucesso!";
            return RedirectToAction("Index");
        }
    }
}