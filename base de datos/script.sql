--creo la base de datos de ventas--------------------------------------------------------------------------
if exists(Select * FROM SysDataBases WHERE name='Ventas')
BEGIN
	DROP DATABASE Ventas
END
go

CREATE DATABASE Ventas

go

--pone en uso la bd----------------------------------------------------------------------------------------
USE Ventas
go

--creo las tablas------------------------------------------------------------------------------------------
CREATE TABLE Usuarios(
	Usulog varchar(10) not null PRIMARY KEY,
	PassLog varchar(5) not null
)
GO

CREATE TABLE Articulos(
	CodArt int NOT NULL PRIMARY KEY ,
	NomArt varchar(20) NOT NULL,
	PreArt Money
) 
go

CREATE TABLE Factura(
	NumFac int NOT NULL PRIMARY KEY,
	FechaFac DATETIME NOT NULL DEFAULT getdate(),
	CodArt int NOT NULL FOREIGN KEY REFERENCES Articulos(CodArt),
	Cantidad int NOT NULL,
	UsuLog varchar(10) not null FOREIGN KEY REFERENCES Usuarios(Usulog)
)
go



--creacion de usuario IIS para que el sitio pueda acceder a la bd-------------------------------------------
USE master
GO

CREATE LOGIN [IIS APPPOOL\DefaultAppPool] FROM WINDOWS 
GO

USE Ventas
GO

CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool]
GO

GRANT Execute to [IIS APPPOOL\DefaultAppPool]
go


----------------------------------------------------------------------------------------------------------
-- Sp de logueo
Create Procedure Logueo @UsuL varchar(10), @PassL varchar(5) AS
Begin
	Select * from Usuarios Where Usulog = @UsuL AND PassLog = @PassL
End
go 

Create Procedure BuscarUsuario @usu varchar(10) AS
Begin
	Select * from Usuarios Where Usulog = @usu
End
go

--creo SP de alta
Create Procedure AltaArticulo @cod int, @nom varchar(20), @pre money AS
Begin
	if (exists(select * from Articulos where codArt = @cod))
		return -1
	else
	begin
		Insert Articulos(CodARt, NomArt, PreArt) Values (@cod, @nom, @pre)
		return 1
	end
end
go

--creo SP de Baja
Create Procedure BajaArticulo @cod int AS
Begin
	if (not exists(select * from Articulos where codArt = @cod))
		return -1
	
	If (exists(select * from Factura where CodArt = @cod))
		return -2
	begin
		Delete From Articulos where codArt = @cod
		return 1
	end
end
go

--creo SP de modificacion
Create Procedure ModArticulo @cod int, @nom varchar(20), @pre money AS
Begin
	if (not exists(select * from Articulos where codArt = @cod))
		return -1
	else
	begin
		Update Articulos Set NomArt=@nom, PreArt=@pre where codArt = @cod
		return 1
	end
end
go

--creo SP de busqueda
Create Procedure BuscoArticulo @cod int AS
Begin
	Select *
	From Articulos
	where codArt = @cod
end
go

--creo SP de listado
Create Procedure ListoArticulo AS
Begin
	Select *
	From Articulos
end
go


--CREO SP ALTA FACTURA
CREATE PROC AltaFactura @numFac INT, @codArt INT, @cantidad INT, @usu varchar(10) 
AS
BEGIN
	if not exists(select * from Articulos where codArt = @codArt)
		return -1
	if not exists(select * from Usuarios where Usulog =@usu )
		return -3
	if exists(select * from Factura where NumFac = @numFac)
		return -2
	
		
	INSERT Factura (NumFac, CodArt, Cantidad, UsuLog)
	VALUES(@numFac,@codArt, @cantidad, @usu)
	
	IF @@ERROR <> 0
		RETURN -4
	ELSE
		RETURN 1

END
GO 


--creo SP de listado
Create Procedure ListoFacturas AS
Begin
	Select *
	From Factura
end
go

--ingreso datos de prueba ----------------------------------------------------------------------------
Exec AltaArticulo 23, "Licuadora", 2500
go
Exec AltaArticulo 48, "Maquina de Coser", 6700
go
Exec AltaArticulo 514, "Ventilador de Techo Vertical", 250
go

INSERT Usuarios(Usulog, PassLog) Values('Usu1', '12345')
INSERT Usuarios(Usulog, PassLog) Values('Usu2', '54321')
go

exec AltaFactura 1, 23, 10, 'Usu1'
go
