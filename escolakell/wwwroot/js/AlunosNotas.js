// wwwroot/js/AlunosNotas.js
function mostrarRecuperacao() {
    console.log("Filtrando alunos em recuperação...");

    const linhas = document.querySelectorAll("tbody tr");
    let contador = 0;

    linhas.forEach(linha => {
        // data-aprovado já é 'true' ou 'false' (bool convertido para string)
        const aprovado = linha.getAttribute("data-aprovado");

        if (aprovado === 'false') {
            linha.style.display = '';
            contador++;
        } else {
            linha.style.display = 'none';
        }
    });

    console.log(`${contador} aluno(s) em recuperação`);
}

function mostrarTodos() {
    console.log("Mostrando todos os alunos...");

    const linhas = document.querySelectorAll("tbody tr");
    linhas.forEach(linha => {
        linha.style.display = '';
    });

    console.log(`${linhas.length} alunos totais`);
}