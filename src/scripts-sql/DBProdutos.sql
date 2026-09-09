CREATE TABLE Produtos (
    Id INT IDENTITY(1,1) NOT NULL,
    CodigoBarras VARCHAR(13) NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Preco NUMERIC(19, 4) NOT NULL,
    CONSTRAINT PK_Produtos PRIMARY KEY (Id)
);

INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000001', 'Panela de Pressão', 51.25);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000002', 'Liquidificador', 53.80);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000003', 'Batedeira', 56.40);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000004', 'Cafeteira Elétrica', 58.95);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000005', 'Torradeira', 60.10);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000006', 'Espremedor de Frutas', 62.75);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000007', 'Sanduicheira', 65.20);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000008', 'Ferro de Passar', 67.85);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000009', 'Aspirador de Pó', 70.00);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000010', 'Ventilador', 72.50);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000011', 'Mixer', 75.15);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000012', 'Chaleira Elétrica', 77.60);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000013', 'Forno Elétrico', 80.25);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000014', 'Grill Elétrico', 83.10);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000015', 'Fritadeira Sem Óleo', 85.75);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000016', 'Processador de Alimentos', 88.20);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000017', 'Purificador de Água', 90.95);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000018', 'Afiador de Facas', 93.50);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000019', 'Espagueteira', 96.10);
INSERT INTO Produtos (CodigoBarras, Nome, Preco) VALUES ('1000000000020', 'Cortador de Legumes', 99.99);