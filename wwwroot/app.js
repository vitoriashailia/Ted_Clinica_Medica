const lista = document.querySelector('#lista');
const form = document.querySelector('#form-consulta');
const mensagem = document.querySelector('#mensagem');

const campoEspecialidade = document.querySelector('#form-especialidade');
const campoMedico = document.querySelector('#form-medico');
let catalogo = { especialidades: [], medicos: {}, convenios: [], exames: [] };

function preencher(select, opcoes, textoInicial) {
  select.innerHTML = '';
  if (textoInicial !== undefined) select.add(new Option(textoInicial, ''));
  for (const opcao of opcoes) select.add(new Option(opcao, opcao));
}

function atualizarMedicos() {
  const selecionado = campoMedico.value;
  let medicos = catalogo.medicos[campoEspecialidade.value];
  if (!medicos) {
    // Nenhuma especialidade escolhida: mostra todos os médicos.
    medicos = [];
    for (const especialidade of catalogo.especialidades) medicos.push(...catalogo.medicos[especialidade]);
  }
  preencher(campoMedico, medicos, 'Selecione o médico');
  if (medicos.includes(selecionado)) campoMedico.value = selecionado;
}

function escolherMedico() {
  // Ao escolher um médico sem especialidade definida, preenche a especialidade dele.
  if (!campoMedico.value || campoEspecialidade.value) return;
  for (const especialidade of catalogo.especialidades) {
    if (catalogo.medicos[especialidade].includes(campoMedico.value)) {
      campoEspecialidade.value = especialidade;
      atualizarMedicos();
      return;
    }
  }
}

function preencherFormulario() {
  preencher(campoEspecialidade, catalogo.especialidades, 'Selecione a especialidade');
  preencher(document.querySelector('#form-convenio'), catalogo.convenios);
  preencher(document.querySelector('#form-exame'), catalogo.exames, 'Nenhum');
  atualizarMedicos();
}

async function carregarCatalogo() {
  catalogo = await (await fetch('/api/catalogo')).json();
  preencherFormulario();
  preencher(document.querySelector('#filtro-especialidade'), catalogo.especialidades, 'Todas');
}

async function carregarConsultas() {
  const parametros = new URLSearchParams({
    busca: document.querySelector('#busca').value,
    campo: document.querySelector('#campo').value,
    especialidade: document.querySelector('#filtro-especialidade').value,
    status: document.querySelector('#status').value,
    ordem: document.querySelector('#ordem').value
  });
  const resposta = await fetch(`/api/consultas?${parametros}`);
  const consultas = await resposta.json();
  lista.innerHTML = '';
  document.querySelector('#total').textContent = `${consultas.length} ${consultas.length === 1 ? 'consulta' : 'consultas'}`;
  document.querySelector('#vazio').hidden = consultas.length > 0;

  for (const consulta of consultas) {
    const linha = document.createElement('tr');
    const data = new Date(consulta.dataHora).toLocaleString('pt-BR');
    linha.innerHTML = `<td>${escapar(consulta.paciente)}</td><td>${escapar(consulta.medico)}</td><td>${escapar(consulta.especialidade || '—')}</td><td>${escapar(consulta.convenio || '—')}</td><td>${escapar(consulta.exame || '—')}</td><td>${data}</td><td><span class="status-tag">${escapar(consulta.status)}</span></td><td><button class="excluir" data-id="${consulta.id}" aria-label="Excluir consulta">Excluir</button></td>`;
    lista.appendChild(linha);
  }
}

function escapar(valor) {
  const elemento = document.createElement('span');
  elemento.textContent = valor;
  return elemento.innerHTML;
}

form.addEventListener('submit', async evento => {
  evento.preventDefault();
  mensagem.textContent = '';
  const dados = Object.fromEntries(new FormData(form));
  dados.dataHora = new Date(dados.dataHora).toISOString();
  const resposta = await fetch('/api/consultas', {
    method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(dados)
  });
  if (!resposta.ok) {
    const erro = await resposta.json(); mensagem.textContent = erro.mensagem; return;
  }
  form.reset(); preencherFormulario(); await carregarCatalogo().then(carregarConsultas);
});

lista.addEventListener('click', async evento => {
  const botao = evento.target.closest('[data-id]');
  if (!botao || !confirm('Deseja excluir esta consulta?')) return;
  await fetch(`/api/consultas/${botao.dataset.id}`, { method: 'DELETE' });
  await carregarCatalogo().then(carregarConsultas);
});

document.querySelector('#busca').addEventListener('input', carregarConsultas);
document.querySelector('#campo').addEventListener('change', carregarConsultas);
document.querySelector('#filtro-especialidade').addEventListener('change', carregarConsultas);
campoEspecialidade.addEventListener('change', atualizarMedicos);
campoMedico.addEventListener('change', escolherMedico);
document.querySelector('#status').addEventListener('change', carregarConsultas);
document.querySelector('#ordem').addEventListener('change', carregarConsultas);
carregarCatalogo().then(carregarConsultas);
