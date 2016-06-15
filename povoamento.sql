INSERT INTO Questao(id_quest, pergunta) VALUES
(1,'Condições de acesso ao estabelecimento?'),
(2,'Condições de armazenamento da matéria prima?'),
(3,'Estado de conservação da matéria prima?'),
(4,'Estado dos equipamentos de refrigeração e congelados?'),
(5,'Estado dos restantes equipamentos?'),
(6,'Condições da área de preparação/cozinha?'),
(7,'Gestão de lixos e resíduos?'),
(8,'Condições da área de lavagem/copa?'),
(9,'Condições da sala de refeições?'),
(10,'Condições das instalações sanitárias?'),
(11,'Vestuário e higiene do pessoal?');

INSERT INTO Fiscal(id_fisc,pass) VALUES
(1,'123'),
(2,'123');

INSERT INTO Estabelecimento(id_est, latitude, morada, nome, longitude) VALUES
(1, 41.53468, 'Rua do Bairro, Braga', 'DankBraga', -8.482436),
(2, 41.15794, 'Av. Boavista, Porto', 'DankPorto', -8.629105);

INSERT INTO Plano(id_plano, disponivel, fiscalCriador) VALUES
(1, 1, 1);

INSERT INTO Visita(id_vis, plano, estabelecimento, concluido) VALUES
(1, 1, 1, 0),
(2, 1, 2, 0);

INSERT INTO VisitaQuestao(questao, visita, resposta) VALUES
(1,1,'Boas condições, incluindo para deficientes'),
(2,1,'Despensa desorganizada. Má iluminação.'),
(3,1,'Em geral boa. Prazo dos enlatados em vias de expirar'),
(4,1,'Bom.'),
(5,1,'Bom.'),
(6,1,'Bom.'),
(7,1,'Sem separação/reciclagem.'),
(8,1,'Necessita de maior espaço. Muito próxima da área de preparação'),
(9,1,'Muito Boa.'),
(10,1,'Boa limpeza.'),
(11,1,'Alguns funcionários sem farda.');

INSERT INTO Nota(id_nota, descricao, text_file, visita) VALUES
(1, 'limpeza do wc', 'plano de limpeza com datas e responsáveis não disponível', 1);

INSERT INTO Foto(id_foto, descricao, foto_file, visita) VALUES
(1, 'vista geral da cozinha', (SELECT * FROM OPENROWSET(BULK N'C:\cozinha.jpg', SINGLE_BLOB) AS src1), 1),
(2, 'sala de refeições', (SELECT * FROM OPENROWSET(BULK N'C:\sala.jpg', SINGLE_BLOB) AS src2), 1);

INSERT INTO Voz(id_voz, descricao, voz_file, visita) VALUES
(1, 'espetadas da madeira', (SELECT * FROM OPENROWSET(BULK N'C:\espetada.mp3', SINGLE_BLOB) AS src1), 1);





