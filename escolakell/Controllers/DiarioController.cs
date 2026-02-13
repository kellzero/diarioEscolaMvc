
using EscolaApp.Repositorio;
using escolakell.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscolaApp.Controllers
{
    public class DiarioController : Controller
    {
        private readonly AlunoRepositorio _repository;

        public DiarioController(AlunoRepositorio repository)
        {
            _repository = repository;
        }

        // GET: Diario/Index
        [HttpGet]
        public IActionResult Index()
        {
            var alunos = _repository.ListarTodos();
            return View(alunos);
        }

        // GET: Diario/Salvar (para Criar ou Editar)
        [HttpGet]
        public IActionResult Salvar(int? id)
        {
            if (id.HasValue && id > 0)
            {
                // MODO EDIÇÃO: Busca aluno existente
                var aluno = _repository.ObterPorId(id.Value);
                if (aluno == null)
                {
                    return NotFound();
                }
                ViewBag.Modo = "Editar";
                return View(aluno);
            }
            else
            {
                // MODO CRIAÇÃO: Novo aluno
                ViewBag.Modo = "Criar";
                return View(new Aluno());
            }
        }

        // POST: Diario/Salvar (para Criar ou Editar)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                if (aluno.Id > 0)
                {
                    // MODO EDIÇÃO: Atualiza
                    _repository.Atualizar(aluno);
                    TempData["Mensagem"] = "Aluno atualizado com sucesso!";
                }
                else
                {
                    // MODO CRIAÇÃO: Adiciona novo
                    _repository.Adicionar(aluno);
                    TempData["Mensagem"] = "Aluno cadastrado com sucesso!";
                }
                return RedirectToAction("Index");
            }

            // Se houver erro, retorna para a view
            ViewBag.Modo = aluno.Id > 0 ? "Editar" : "Criar";
            return View(aluno);
        }

        // GET: Diario/Excluir/{id} (confirmação)
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

        // POST: Diario/Excluir/{id} (ação real)
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