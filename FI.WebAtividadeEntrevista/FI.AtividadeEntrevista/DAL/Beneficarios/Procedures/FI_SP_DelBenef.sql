﻿CREATE PROCEDURE FI_SP_DelBenef
    @Id BIGINT
AS 
BEGIN
    DELETE BENEFICIARIOS  
    WHERE ID = @Id
END