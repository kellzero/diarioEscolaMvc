using EscolaApp.Repositorio;
using escolakell.Models;
using escolakell.Repositorio;
using Microsoft.AspNetCore.Mvc;
using escolakell.Models.ViewModel;

namespace escolakell.Controllers
{
    public class AlunoController : Controller
    {
        private readonly AlunoRepositorio _repository;
        private readonly DiarioRepositorio _diarioRepositorio;
        private readonly ILogger<AlunoController> _logger;

        public AlunoController(IConfiguration configuration, ILogger<AlunoController> logger, DiarioRepositorio diarioRepositorio)
        {
            _logger = logger;

            // Criando logger para o repositório
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var repoLogger = loggerFactory.CreateLogger<AlunoRepositorio>();

            _repository = new AlunoRepositorio(configuration, repoLogger);
            _diarioRepositorio = diarioRepositorio;
        }

        public IActionResult Index()
        {
            var alunos = _repository.ListarTodos();
            return View(alunos);
        }

        [HttpGet]
        public IActionResult Salvar(int? id)
        {
            var viewModel = new AlunoDiarioViewModel();

            if (id.HasValue)
            {
                viewModel.Aluno = _repository.ObterPorId(id.Value);

                viewModel.Diario = _diarioRepositorio.ObterUltimaNotaPorAlunoId(id.Value)
                                   ?? new Diario();

                ViewBag.Modo = "Editar";
            }
            else
            {
                viewModel.Aluno = new Aluno();  
                viewModel.Diario = new Diario(); 
                ViewBag.Modo = "Criar";
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(AlunoDiarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (viewModel.Aluno.Id > 0)
                    {
                        _repository.Atualizar(viewModel.Aluno);
                        TempData["Mensagem"] = "Aluno atualizado com sucesso!";
                    }
                    else
                    {
                        var novoId = _repository.Adicionar(viewModel.Aluno);
                        viewModel.Aluno.Id = novoId;
                        TempData["Mensagem"] = "Aluno cadastrado com sucesso!";
                    }

                    viewModel.Diario.AlunoId = viewModel.Aluno.Id;

                    viewModel.Diario.Media = (viewModel.Diario.Nota1 + viewModel.Diario.Nota2) / 2;
                    viewModel.Diario.SituacaoAprovada = viewModel.Diario.Media >= 6;

                    var notaExistente = _diarioRepositorio.ObterPorAlunoIdEMateria(
                        viewModel.Aluno.Id,
                        viewModel.Diario.Materia);

                    if (notaExistente != null)
                    {
                        viewModel.Diario.Id = notaExistente.Id;
                        _diarioRepositorio.Atualizar(viewModel.Diario);
                    }
                    else
                    {
                        _diarioRepositorio.Adicionar(viewModel.Diario);
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao salvar aluno");
                    ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
                }
            }

            ViewBag.Modo = viewModel.Aluno.Id > 0 ? "Editar" : "Criar";
            return View(viewModel);
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
            try
            {
               
                _repository.Excluir(id);
                TempData["Mensagem"] = "Aluno excluído com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir aluno");
                TempData["Erro"] = "Erro ao excluir: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}