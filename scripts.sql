//scripts para base de datos
CREATE TABLE status (
    ID INT PRIMARY KEY,
    Descripcion NVARCHAR(50)
);

INSERT INTO status (ID, Descripcion) VALUES (1, 'Asignado');
INSERT INTO status (ID, Descripcion) VALUES (2, 'Removido');
INSERT INTO status (ID, Descripcion) VALUES (3, 'Pendiente');

CREATE TABLE dbo.employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,
    DPI VARCHAR(20) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Sexo CHAR(1) NOT NULL,
    FechaIngresoEmpresa DATE NOT NULL,
    Edad AS DATEDIFF(YEAR, FechaNacimiento, GETDATE()) - 
          CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, FechaNacimiento, GETDATE()), FechaNacimiento) > GETDATE() THEN 1 ELSE 0 END,
    Direccion NVARCHAR(MAX),
    NIT VARCHAR(15),
    CodigoDepartamento INT NOT NULL,
    Status INT NOT NULL,
    CONSTRAINT FK_Departamento_Employee FOREIGN KEY (CodigoDepartamento) REFERENCES dbo.ca_departamentoArea(codigoDepartamento),
    CONSTRAINT FK_Status FOREIGN KEY (Status) REFERENCES status(ID)
);

