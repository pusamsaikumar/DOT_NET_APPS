USE RSA_GroceryDEV

select * from SSNews
select ClubId,createddate from clubs where Name='$5 off your NEXT visit1' AND ClubId=1
select ClubId,UserId from clubUsers where CreatedDate='2018-09-18 01:45:53.867' AND ClubId=1



---- CREATE UPC PROMOTIONS:
exec dbo.RSA_ETL_CouponProducts @StartDate='',@EndDate=''
exec dbo.RSA_ETL_Products @StartDate='',@EndDate=''
SELECT * FROM Product;
select * from SSNews_Products;



EXEC [dbo].[SaveUPCPromotions]
    @NewsID = 0, 
    @NewsCategoryID = 4, 
    @Title = 'Test demo Coupon', 
    @Details = N'Detailed description of the coupon', 
    @ImagePath = 'path/to/image.jpg', 
    @ValidFromDate = '2025-01-01 00:00:00', 
    @ExpiresOn = '2025-12-31 23:59:59', 
    @SendNotification = 1, 
    @CustomerID = 456, 
    @CreateUserID = 789, 
    @UpdateUserID = 789, 
    @PUICode = 'PUI123', 
    @ProductId = 101, 
    @Amount = 50.00, 
    @DiscountAmount = 5.00, 
    @IsDiscountPercentage = 1, 
    @NCRPromotionCode = 'PROMO123', 
    @IsItStoreSpecific = 0, 
    @ManufacturerCouponId = 10000000001, 
    @ProductQuantity = 10, 
    @UpSellProductId = 102, 
    @UpSellProductQuantity = 5, 
    @IsFeatured = 1, 
    @DeleteFlag = 0, 
    @IsItTargetSpecific = 1, 
    @OtherDetails = 'Extra details for the coupon', 
    @IsRecurring = 0, 
    @MfgShutOffDate = '2025-11-01 00:00:00', 
    @IsDealOftheWeek = 1, 
    @News_Id = '', 
    @DepartmentId = '1,2,3', 
    @IsMajorDepartment = 1, 
    @StoreID = '4', 
    @PageNumber = 1, 
    @PdfFileName = 'coupon.pdf', 
    @Id = 0, 
    @StoreRouteId = 'route1', 
    @ClientStoreId = 4, 
    @RecurringStartDate = '2025-01-01 00:00:00', 
    @RecurringEndDate = '2025-12-31 23:59:59', 
    @RecurringTypeId = 1,
    @ClubIds = '1,3',
    @GroupNames = N'$5 off your NEXT visit1,$5 off your NEXT visit1',
    @ClientStoreIds = '4',
    @UPC = '9733900020',
    @ProductName = 'Valentina Marisc',
    @Product_ID = 0;





EXEC [dbo].[SaveUPCPromotions]
    @NewsID = 0, 
    @NewsCategoryID = 4, 
    @Title = ' UPC Coupon', 
    @Details = N'Detailed description of the coupon', 
    @ImagePath = 'path/to/image.jpg', 
    @ValidFromDate = '2025-01-01 00:00:00', 
    @ExpiresOn = '2025-12-31 23:59:59', 
    @SendNotification = 1, 
    @CustomerID = 1, 
    @CreateUserID = 789, 
    @UpdateUserID = 789, 
    @PUICode = 'PUI123', 
    @ProductId = 0, 
    @Amount = 50.00, 
    @DiscountAmount = 5.00, 
    @IsDiscountPercentage = 1, 
    @NCRPromotionCode = 'PROMO123', 
    @IsItStoreSpecific = 0, 
    @ManufacturerCouponId = 10000000001, 
    @ProductQuantity = 10, 
    @UpSellProductId = 102, 
    @UpSellProductQuantity = 5, 
    @IsFeatured = 1, 
    @DeleteFlag = 0, 
    @IsItTargetSpecific = 1, 
    @OtherDetails = 'Extra details for the coupon', 
    @IsRecurring = 0, 
    @MfgShutOffDate = '2025-11-01 00:00:00', 
    @IsDealOftheWeek = 1, 
    @News_Id = '', 
    @DepartmentId = '1,2,3', 
    @IsMajorDepartment = 1, 
    @StoreID = '4', 
    @PageNumber = 1, 
    @PdfFileName = 'coupon.pdf', 
    @Id = 0, 
    @StoreRouteId = 'route1', 
    @ClientStoreId = 4, 
    @RecurringStartDate = '2025-01-01 00:00:00', 
    @RecurringEndDate = '2025-12-31 23:59:59', 
    @RecurringTypeId = 1,
    @ClubIds = '1,3',
    @GroupNames = N'$5 off your NEXT visit1,$5 off your NEXT visit1',
    @ClientStoreIds = '4',
    @UPC = '9733900020',
    @ProductName = 'Valentina Marisc',
    @Product_ID = 1;



-----  INSERT UPDATE UPCS COUPONS:

-------- 
ALTER PROC [dbo].[SaveUPCPromotions]
    @NewsID INT,
    @NewsCategoryID INT,
    @Title VARCHAR(200),
    @Details NVARCHAR(MAX),
    @ImagePath VARCHAR(200),
    @ValidFromDate DATETIME,
    @ExpiresOn DATETIME,
    @SendNotification BIT,
    @CustomerID INT,
    @CreateUserID INT,
    @UpdateUserID INT,
    @PUICode VARCHAR(25),
    @ProductId INT,
    @Amount MONEY,
    @DiscountAmount MONEY,
    @IsDiscountPercentage BIT,
    @NCRPromotionCode VARCHAR(50),
    @IsItStoreSpecific BIT,
    @ManufacturerCouponId BIGINT,
    @ProductQuantity INT,
    @UpSellProductId INT,
    @UpSellProductQuantity INT,
    @IsFeatured BIT,
    @DeleteFlag BIT,
    @IsItTargetSpecific BIT,
    @OtherDetails VARCHAR(300),
    @IsRecurring BIT,
    @MfgShutOffDate DATETIME,
    @IsDealOftheWeek BIT,
    @News_Id VARCHAR(10) OUT,
    @DepartmentId VARCHAR(MAX), -- New parameter for department IDs
    @IsMajorDepartment BIT,
    @StoreID VARCHAR(MAX), -- New parameter for Store IDs
    @PageNumber INT,
    @PdfFileName VARCHAR(200),
    @Id INT,
    @StoreRouteId VARCHAR(50),
    @ClientStoreId INT,
	@RecurringStartDate DATETIME,
	@RecurringEndDate DATETIME,
	@RecurringTypeId INT,
	 @ClubIds NVARCHAR(MAX),
    @GroupNames NVARCHAR(MAX),
	@ClientStoreIds NVARCHAR(MAX),
	@UPC VARCHAR(MAX),
    @ProductName VARCHAR(MAX),
	@Product_ID INT

AS
BEGIN
    SET NOCOUNT ON;

    -- Check for deletion
    IF @DeleteFlag = 1
    BEGIN
        UPDATE SSNews
        SET NewsStatusID = 2
        WHERE NewsID = @NewsID;

        RETURN;
    END

    -- If record exists, update it
    IF EXISTS(SELECT 1 FROM SSNews WHERE NewsID = @NewsID)
    BEGIN
        UPDATE SSNews
        SET 
            NewsCategoryID = @NewsCategoryID,
            Title = @Title,
            Details = @Details,
            ImagePath = @ImagePath,
            ValidFromDate = @ValidFromDate,
            ExpiresOn = @ExpiresOn,
            SendNotification = @SendNotification,
            CustomerID = @CustomerID,
            UpdateUserID = @UpdateUserID,
            PUICode = @PUICode,
            Amount = @Amount,
            DiscountAmount = @DiscountAmount,
            IsDiscountPercentage = @IsDiscountPercentage,
            IsItStoreSpecific = @IsItStoreSpecific,
            IsFeatured = @IsFeatured,
            IsItTargetSpecific = @IsItTargetSpecific,
            OtherDetails = @OtherDetails,
            IsRecurring = @IsRecurring,
            UpdateDateTime = GETDATE(),
            ProductQuantity = @ProductQuantity,
            UpSellProductId = @UpSellProductId,
            UpSellProductQuantity = @UpSellProductQuantity,
            MfgShutOffDate = CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE @ExpiresOn END,
            IsDealOftheWeek = @IsDealOftheWeek
        WHERE NewsID = @NewsID;
    END
    ELSE
    BEGIN
        -- Insert a new record
        INSERT INTO SSNews(
            NewsCategoryID, Title, Details, ImagePath, ValidFromDate, ExpiresOn, SendNotification,
            CustomerID, CreateUserID, CreateDateTime, UpdateUserID, UpdateDateTime,
            PUICode, ProductId, Amount, DiscountAmount, IsDiscountPercentage, NCRPromotionCode,
            IsItStoreSpecific, ManufacturerCouponId, ProductQuantity, UpSellProductId,
            UpSellProductQuantity, IsFeatured, IsItTargetSpecific, OtherDetails, IsRecurring,
            MfgShutOffDate, IsDealOftheWeek
        )
        VALUES (
            @NewsCategoryID, @Title, @Details, @ImagePath, @ValidFromDate, @ExpiresOn, @SendNotification,
            @CustomerID, @CreateUserID, GETDATE(), @UpdateUserID, GETDATE(),
            @PUICode, @ProductId, @Amount, @DiscountAmount, @IsDiscountPercentage, @NCRPromotionCode,
            @IsItStoreSpecific, @ManufacturerCouponId, @ProductQuantity, @UpSellProductId,
            @UpSellProductQuantity, @IsFeatured, @IsItTargetSpecific, @OtherDetails, @IsRecurring,
            CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE @ExpiresOn END, @IsDealOftheWeek
        );

        -- Get the new record ID
        SET @News_Id = CAST(SCOPE_IDENTITY() AS VARCHAR(10));

		-- Insert into SSNews_Departments
    IF @DepartmentId IS NOT NULL
    BEGIN
        INSERT INTO SSNews_Departments(NewsId, DepartmentId, CreatedDate, IsMajorDepartment)
        SELECT @News_Id, Value, GETDATE(), @IsMajorDepartment
        FROM STRING_SPLIT(@DepartmentId, ',');
    END

	----------------- INSERT PRODUCTS START SSNews_Products  BY UPCS -------------

	IF(@Product_ID = 0)
	BEGIN
		  SET @ProductId = (Select top 1 ProductId from product where productcode = @UPC)

		  IF(@ProductId = 0)
		  BEGIN
	     
			 -- If product upc is not found then insert and get productid
			   EXEC [dbo].[SaveProducts]  0, 
								@UPC, 
								1,
								1, 
								@ProductName, 
								@ProductName, 
								1, 
								1,
								1, 
								 1,
								 @UPC, 
								 1, 
								 1, 
								 1,
								 0,
								 'CompanyLogo.png',
								 '',
								 '',
								 1,
								 ''

				  SET @ProductId = (Select top 1 ProductId from product where productcode = @UPC)
		  END
	END




	IF EXISTS(SELECT *  FROM SSNews_Products WHERE ProductId = @ProductId AND NewsId = @News_Id)
	BEGIN
		UPDATE	SSNews_Products
		SET		
				NewsId = @News_Id,
				ProductId = @ProductId,
				CreatedDate = getdate()
		WHERE	ProductId = @ProductId  AND NewsId = @News_Id;
	END
	ELSE
	BEGIN
		INSERT INTO SSNews_Products(
		                NewsId, 
					    ProductId,
						CreatedDate 
				   )
				   VALUES(
				        @News_Id, 
					    @ProductId,
						GETDATE()
						)

		         
	END
	---------------- INSERT PRODUCTS SSNews_Products END BY UPCS ---------- ---------
	-------------- INSERT PRODUCTS Products  start    ---------------

	--IF EXISTS(SELECT *  FROM SSNews_Products WHERE ProductId = @ProductId AND NewsId = @News_Id)
	--BEGIN
	--	UPDATE	SSNews_Products
	--	SET		
	--			NewsId = @News_Id,
	--			ProductId = @ProductId,
	--			CreatedDate = getdate()
	--	WHERE	ProductId = @ProductId  AND NewsId = @News_Id;
	--END
	--ELSE
	--BEGIN
	--	INSERT INTO SSNews_Products(
	--	                NewsId, 
	--				    ProductId,
	--					CreatedDate 
	--			   )
	--			   VALUES(
	--			        @News_Id, 
	--				    @ProductId,
	--					GETDATE()
	--					)

		         
	--END

	-----------------       INSERT PRODUCTS Products  start                   ----------
	 -- Insert into WeeklyAddPDF
    IF @StoreID IS NOT NULL
    BEGIN
        INSERT INTO WeeklyAddPDF(StoreID, PageNumber, ValidFromDate, Expireson, PdfFileName, CreatedDate, ModifiedDate)
        SELECT Value, @PageNumber, @ValidFromDate, @ExpiresOn, @PdfFileName, GETDATE(), GETDATE()
        FROM STRING_SPLIT(@StoreID, ',');
    END

	 -- Insert into ClubUsers
    --IF @ClubId IS NOT NULL AND @UserDetailId IS NOT NULL
    --BEGIN
    --    INSERT INTO ClubUsers(ClubId, UserId, ClubMemberId, CreatedDate)
    --    VALUES (@ClubId, @UserDetailId, @ClubMemberId, GETDATE());
    --END


	  DECLARE @ClubIdTable TABLE (ClubId INT);
        DECLARE @GroupNameTable TABLE (GroupName NVARCHAR(100));

        -- Split ClubIds
        INSERT INTO @ClubIdTable (ClubId)
        SELECT value
        FROM STRING_SPLIT(@ClubIds, ',');

        -- Split GroupNames
        INSERT INTO @GroupNameTable (GroupName)
        SELECT value
        FROM STRING_SPLIT(@GroupNames, ',');

        -- Step 2: Process each combination of ClubId and GroupName
        DECLARE @CurrentClubId INT;
        DECLARE @CurrentGroupName NVARCHAR(100);
        DECLARE @CreatedDate DATETIME;
        DECLARE @UserId INT;

        -- Cursor for processing ClubIds and GroupNames
        DECLARE ClubCursor CURSOR FOR
        SELECT ClubId, GroupName
        FROM @ClubIdTable
        CROSS JOIN @GroupNameTable;

        OPEN ClubCursor;

        FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Retrieve CreatedDate from the clubs table
            SELECT TOP 1 
                @CreatedDate = CreatedDate
            FROM clubs
            WHERE ClubId = @CurrentClubId AND Name = @CurrentGroupName;

            -- If no matching record, skip to the next iteration
            IF @CreatedDate IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Retrieve UserId from clubUsers
            SELECT TOP 1
                @UserId = UserId
            FROM clubUsers
            WHERE ClubId = @CurrentClubId AND CreatedDate = @CreatedDate;

            -- If no matching user, skip to the next iteration
            IF @UserId IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Insert data into ClubUser table
            INSERT INTO ClubUsers(ClubId, UserId, ClubMemberId, CreatedDate)
            VALUES (@CurrentClubId, @UserId, NEWID(), GETDATE());

            FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
        END;

        CLOSE ClubCursor;
        DEALLOCATE ClubCursor;

	----  CludUser end

	---- save Recurrings start
IF @IsRecurring = 1
BEGIN
    IF EXISTS(SELECT * FROM SSNewsRecurrings WHERE NewsId = @NewsId)
    BEGIN
        UPDATE SSNewsRecurrings
           SET RecurringStartDate = @RecurringStartDate,
               RecurringEndDate = @RecurringEndDate,
               RecurringTypeId = @RecurringTypeId
           WHERE NewsId = @NewsId
    END
    ELSE
    BEGIN
        INSERT INTO SSNewsRecurrings 
        VALUES(@NewsId, @RecurringStartDate, @RecurringEndDate, @RecurringTypeId, GetDate())
    END
END

----   storestart
 --IF EXISTS(SELECT 1 FROM SelectedStoresForCoupons WHERE Id = @Id)
 --   BEGIN
 --       UPDATE SelectedStoresForCoupons
 --       SET NewsId = @NewsId, StoreRouteId = @StoreRouteId
 --       WHERE Id = @Id;
 --   END
 --   ELSE
 --   BEGIN
 --       DECLARE @StoreRouteIds VARCHAR(200), @EnterpriseId VARCHAR(200);

 --       IF @ClientStoreId = 0
 --       BEGIN
 --           SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid, @StoreRouteIds = CER.POSRouteId
 --           FROM ClientEnterprises CE
 --           INNER JOIN ClientEnterpriseRoutes CER ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
 --           WHERE CER.IsRetailerlevel = 1;

 --           IF NOT EXISTS(
 --               SELECT 1 FROM SelectedStoresForCoupons 
 --               WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId
 --           )
 --           BEGIN
 --               INSERT INTO SelectedStoresForCoupons(NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId)
 --               VALUES (@NewsId, @StoreRouteIds, GETDATE(), @ClientStoreId, @EnterpriseId);
 --           END
 --       END
 --       ELSE
 --       BEGIN
 --           SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid, @StoreRouteIds = CER.posrouteid
 --           FROM ClientStores CS
 --           INNER JOIN ClientEnterprises CE ON CE.ClientEnterprisesid = CS.cliententerprisesid
 --           INNER JOIN ClientEnterpriseRoutes CER ON CER.ClientEnterpriserouteId = CS.ClientEnterprisesrouteId
 --           WHERE CS.ClientStoreId = @ClientStoreId AND CER.IsRetailerlevel = 0;

 --           IF NOT EXISTS(
 --               SELECT 1 FROM SelectedStoresForCoupons 
 --               WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId
 --           )
 --           BEGIN
 --               INSERT INTO SelectedStoresForCoupons(NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId)
 --               VALUES (@NewsId, @StoreRouteIds, GETDATE(), @ClientStoreId, @EnterpriseId);
 --           END
 --       END
 --   END
-----  
 DECLARE @EnterpriseId VARCHAR(200);

    -- Table to hold split ClientStoreIds
    DECLARE @TempData TABLE (ClientStoreId INT);

    -- Split the ClientStoreIds string into rows
    INSERT INTO @TempData (ClientStoreId)
    SELECT value 
    FROM STRING_SPLIT(@ClientStoreIds, ',');

    -- Iterate through the table of ClientStoreIds
    DECLARE @TempClientStoreId INT;

    DECLARE cur CURSOR FOR
    SELECT ClientStoreId FROM @TempData;

    OPEN cur;

    FETCH NEXT FROM cur INTO @TempClientStoreId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF EXISTS(SELECT 1 FROM SelectedStoresForCoupons WHERE Id = @TempClientStoreId)
        BEGIN
            UPDATE SelectedStoresForCoupons
            SET NewsId = @NewsId, StoreRouteId = @StoreRouteId
            WHERE Id = @TempClientStoreId;
        END
        ELSE
        BEGIN
            IF @TempClientStoreId = 0
            BEGIN
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid
                FROM ClientEnterprises CE
                INNER JOIN ClientEnterpriseRoutes CER 
                ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
                WHERE CER.IsRetailerlevel = 1;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteId AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(
                        NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId
                    )
                    VALUES (@NewsId, @StoreRouteId, GETDATE(), @TempClientStoreId, @EnterpriseId);
                END
            END
            ELSE
            BEGIN
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid
                FROM ClientStores CS
                INNER JOIN ClientEnterprises CE 
                ON CE.ClientEnterprisesid = CS.cliententerprisesid
                INNER JOIN ClientEnterpriseRoutes CER 
                ON CER.ClientEnterpriserouteId = CS.ClientEnterprisesrouteId
                WHERE CS.ClientStoreId = @TempClientStoreId AND CER.IsRetailerlevel = 0;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteId AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(
                        NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId
                    )
                    VALUES (@NewsId, @StoreRouteId, GETDATE(), @TempClientStoreId, @EnterpriseId);
                END
            END
        END

        FETCH NEXT FROM cur INTO @TempClientStoreId;
    END;

    CLOSE cur;
    DEALLOCATE cur;
----  stores end


    END   
END;
CREATE PROC [dbo].[SaveUPCPromotions]
    @NewsID INT,
    @NewsCategoryID INT,
    @Title VARCHAR(200),
    @Details NVARCHAR(MAX),
    @ImagePath VARCHAR(200),
    @ValidFromDate DATETIME,
    @ExpiresOn DATETIME,
    @SendNotification BIT,
    @CustomerID INT,
    @CreateUserID INT,
    @UpdateUserID INT,
    @PUICode VARCHAR(25),
    @ProductId INT,
    @Amount MONEY,
    @DiscountAmount MONEY,
    @IsDiscountPercentage BIT,
    @NCRPromotionCode VARCHAR(50),
    @IsItStoreSpecific BIT,
    @ManufacturerCouponId BIGINT,
    @ProductQuantity INT,
    @UpSellProductId INT,
    @UpSellProductQuantity INT,
    @IsFeatured BIT,
    @DeleteFlag BIT,
    @IsItTargetSpecific BIT,
    @OtherDetails VARCHAR(300),
    @IsRecurring BIT,
    @MfgShutOffDate DATETIME,
    @IsDealOftheWeek BIT,
    @News_Id VARCHAR(10) OUT,
    @DepartmentId VARCHAR(MAX), -- New parameter for department IDs
    @IsMajorDepartment BIT,
    @StoreID VARCHAR(MAX), -- New parameter for Store IDs
    @PageNumber INT,
    @PdfFileName VARCHAR(200),
    @Id INT,
    @StoreRouteId VARCHAR(50),
    @ClientStoreId INT,
	@RecurringStartDate DATETIME,
	@RecurringEndDate DATETIME,
	@RecurringTypeId INT,
	 @ClubIds NVARCHAR(MAX),
    @GroupNames NVARCHAR(MAX),
	@ClientStoreIds NVARCHAR(MAX),
	@UPC VARCHAR(MAX),
    @ProductName VARCHAR(MAX),
	@Product_ID INT

AS
BEGIN
    SET NOCOUNT ON;

    -- Check for deletion
    IF @DeleteFlag = 1
    BEGIN
        UPDATE SSNews
        SET NewsStatusID = 2
        WHERE NewsID = @NewsID;

        RETURN;
    END

    -- If record exists, update it
    IF EXISTS(SELECT 1 FROM SSNews WHERE NewsID = @NewsID)
    BEGIN
        UPDATE SSNews
        SET 
            NewsCategoryID = @NewsCategoryID,
            Title = @Title,
            Details = @Details,
            ImagePath = @ImagePath,
            ValidFromDate = @ValidFromDate,
            ExpiresOn = @ExpiresOn,
            SendNotification = @SendNotification,
            CustomerID = @CustomerID,
            UpdateUserID = @UpdateUserID,
            PUICode = @PUICode,
            Amount = @Amount,
            DiscountAmount = @DiscountAmount,
            IsDiscountPercentage = @IsDiscountPercentage,
            IsItStoreSpecific = @IsItStoreSpecific,
            IsFeatured = @IsFeatured,
            IsItTargetSpecific = @IsItTargetSpecific,
            OtherDetails = @OtherDetails,
            IsRecurring = @IsRecurring,
            UpdateDateTime = GETDATE(),
            ProductQuantity = @ProductQuantity,
            UpSellProductId = @UpSellProductId,
            UpSellProductQuantity = @UpSellProductQuantity,
            MfgShutOffDate = CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE @ExpiresOn END,
            IsDealOftheWeek = @IsDealOftheWeek
        WHERE NewsID = @NewsID;
    END
    ELSE
    BEGIN
        -- Insert a new record
        INSERT INTO SSNews(
            NewsCategoryID, Title, Details, ImagePath, ValidFromDate, ExpiresOn, SendNotification,
            CustomerID, CreateUserID, CreateDateTime, UpdateUserID, UpdateDateTime,
            PUICode, ProductId, Amount, DiscountAmount, IsDiscountPercentage, NCRPromotionCode,
            IsItStoreSpecific, ManufacturerCouponId, ProductQuantity, UpSellProductId,
            UpSellProductQuantity, IsFeatured, IsItTargetSpecific, OtherDetails, IsRecurring,
            MfgShutOffDate, IsDealOftheWeek
        )
        VALUES (
            @NewsCategoryID, @Title, @Details, @ImagePath, @ValidFromDate, @ExpiresOn, @SendNotification,
            @CustomerID, @CreateUserID, GETDATE(), @UpdateUserID, GETDATE(),
            @PUICode, @ProductId, @Amount, @DiscountAmount, @IsDiscountPercentage, @NCRPromotionCode,
            @IsItStoreSpecific, @ManufacturerCouponId, @ProductQuantity, @UpSellProductId,
            @UpSellProductQuantity, @IsFeatured, @IsItTargetSpecific, @OtherDetails, @IsRecurring,
            CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE @ExpiresOn END, @IsDealOftheWeek
        );

        -- Get the new record ID
        SET @News_Id = CAST(SCOPE_IDENTITY() AS VARCHAR(10));

		-- Insert into SSNews_Departments
    IF @DepartmentId IS NOT NULL
    BEGIN
        INSERT INTO SSNews_Departments(NewsId, DepartmentId, CreatedDate, IsMajorDepartment)
        SELECT @News_Id, Value, GETDATE(), @IsMajorDepartment
        FROM STRING_SPLIT(@DepartmentId, ',');
    END

	----------------- INSERT PRODUCTS START SSNews_Products  BY UPCS -------------

	IF(@Product_ID = 0)
	BEGIN
		  SET @ProductId = (Select top 1 ProductId from product where productcode = @UPC)

		  IF(@ProductId = 0)
		  BEGIN
	     
			 -- If product upc is not found then insert and get productid
			   EXEC [dbo].[SaveProducts]  0, 
								@UPC, 
								1,
								1, 
								@ProductName, 
								@ProductName, 
								1, 
								1,
								1, 
								 1,
								 @UPC, 
								 1, 
								 1, 
								 1,
								 0,
								 'CompanyLogo.png',
								 '',
								 '',
								 1,
								 ''

				  SET @ProductId = (Select top 1 ProductId from product where productcode = @UPC)
		  END
	END




	IF EXISTS(SELECT *  FROM SSNews_Products WHERE ProductId = @ProductId AND NewsId = @News_Id)
	BEGIN
		UPDATE	SSNews_Products
		SET		
				NewsId = @News_Id,
				ProductId = @ProductId,
				CreatedDate = getdate()
		WHERE	ProductId = @ProductId  AND NewsId = @News_Id;
	END
	ELSE
	BEGIN
		INSERT INTO SSNews_Products(
		                NewsId, 
					    ProductId,
						CreatedDate 
				   )
				   VALUES(
				        @News_Id, 
					    @ProductId,
						GETDATE()
						)

		         
	END
	---------------- INSERT PRODUCTS SSNews_Products END BY UPCS ---------- ---------
	-------------- INSERT PRODUCTS Products  start    ---------------

	-----------------       INSERT PRODUCTS Products  start                   ----------
	 -- Insert into WeeklyAddPDF
    IF @StoreID IS NOT NULL
    BEGIN
        INSERT INTO WeeklyAddPDF(StoreID, PageNumber, ValidFromDate, Expireson, PdfFileName, CreatedDate, ModifiedDate)
        SELECT Value, @PageNumber, @ValidFromDate, @ExpiresOn, @PdfFileName, GETDATE(), GETDATE()
        FROM STRING_SPLIT(@StoreID, ',');
    END

	 -- Insert into ClubUsers
    --IF @ClubId IS NOT NULL AND @UserDetailId IS NOT NULL
    --BEGIN
    --    INSERT INTO ClubUsers(ClubId, UserId, ClubMemberId, CreatedDate)
    --    VALUES (@ClubId, @UserDetailId, @ClubMemberId, GETDATE());
    --END


	  DECLARE @ClubIdTable TABLE (ClubId INT);
        DECLARE @GroupNameTable TABLE (GroupName NVARCHAR(100));

        -- Split ClubIds
        INSERT INTO @ClubIdTable (ClubId)
        SELECT value
        FROM STRING_SPLIT(@ClubIds, ',');

        -- Split GroupNames
        INSERT INTO @GroupNameTable (GroupName)
        SELECT value
        FROM STRING_SPLIT(@GroupNames, ',');

        -- Step 2: Process each combination of ClubId and GroupName
        DECLARE @CurrentClubId INT;
        DECLARE @CurrentGroupName NVARCHAR(100);
        DECLARE @CreatedDate DATETIME;
        DECLARE @UserId INT;

        -- Cursor for processing ClubIds and GroupNames
        DECLARE ClubCursor CURSOR FOR
        SELECT ClubId, GroupName
        FROM @ClubIdTable
        CROSS JOIN @GroupNameTable;

        OPEN ClubCursor;

        FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Retrieve CreatedDate from the clubs table
            SELECT TOP 1 
                @CreatedDate = CreatedDate
            FROM clubs
            WHERE ClubId = @CurrentClubId AND Name = @CurrentGroupName;

            -- If no matching record, skip to the next iteration
            IF @CreatedDate IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Retrieve UserId from clubUsers
            SELECT TOP 1
                @UserId = UserId
            FROM clubUsers
            WHERE ClubId = @CurrentClubId AND CreatedDate = @CreatedDate;

            -- If no matching user, skip to the next iteration
            IF @UserId IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Insert data into ClubUser table
            INSERT INTO ClubUsers(ClubId, UserId, ClubMemberId, CreatedDate)
            VALUES (@CurrentClubId, @UserId, NEWID(), GETDATE());

            FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
        END;

        CLOSE ClubCursor;
        DEALLOCATE ClubCursor;

	----  CludUser end

	---- save Recurrings start
IF @IsRecurring = 1
BEGIN
    IF EXISTS(SELECT * FROM SSNewsRecurrings WHERE NewsId = @NewsId)
    BEGIN
        UPDATE SSNewsRecurrings
           SET RecurringStartDate = @RecurringStartDate,
               RecurringEndDate = @RecurringEndDate,
               RecurringTypeId = @RecurringTypeId
           WHERE NewsId = @NewsId
    END
    ELSE
    BEGIN
        INSERT INTO SSNewsRecurrings 
        VALUES(@NewsId, @RecurringStartDate, @RecurringEndDate, @RecurringTypeId, GetDate())
    END
END

----   storestart
 --IF EXISTS(SELECT 1 FROM SelectedStoresForCoupons WHERE Id = @Id)
 --   BEGIN
 --       UPDATE SelectedStoresForCoupons
 --       SET NewsId = @NewsId, StoreRouteId = @StoreRouteId
 --       WHERE Id = @Id;
 --   END
 --   ELSE
 --   BEGIN
 --       DECLARE @StoreRouteIds VARCHAR(200), @EnterpriseId VARCHAR(200);

 --       IF @ClientStoreId = 0
 --       BEGIN
 --           SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid, @StoreRouteIds = CER.POSRouteId
 --           FROM ClientEnterprises CE
 --           INNER JOIN ClientEnterpriseRoutes CER ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
 --           WHERE CER.IsRetailerlevel = 1;

 --           IF NOT EXISTS(
 --               SELECT 1 FROM SelectedStoresForCoupons 
 --               WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId
 --           )
 --           BEGIN
 --               INSERT INTO SelectedStoresForCoupons(NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId)
 --               VALUES (@NewsId, @StoreRouteIds, GETDATE(), @ClientStoreId, @EnterpriseId);
 --           END
 --       END
 --       ELSE
 --       BEGIN
 --           SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid, @StoreRouteIds = CER.posrouteid
 --           FROM ClientStores CS
 --           INNER JOIN ClientEnterprises CE ON CE.ClientEnterprisesid = CS.cliententerprisesid
 --           INNER JOIN ClientEnterpriseRoutes CER ON CER.ClientEnterpriserouteId = CS.ClientEnterprisesrouteId
 --           WHERE CS.ClientStoreId = @ClientStoreId AND CER.IsRetailerlevel = 0;

 --           IF NOT EXISTS(
 --               SELECT 1 FROM SelectedStoresForCoupons 
 --               WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId
 --           )
 --           BEGIN
 --               INSERT INTO SelectedStoresForCoupons(NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId)
 --               VALUES (@NewsId, @StoreRouteIds, GETDATE(), @ClientStoreId, @EnterpriseId);
 --           END
 --       END
 --   END
-----  
 DECLARE @EnterpriseId VARCHAR(200);

    -- Table to hold split ClientStoreIds
    DECLARE @TempData TABLE (ClientStoreId INT);

    -- Split the ClientStoreIds string into rows
    INSERT INTO @TempData (ClientStoreId)
    SELECT value 
    FROM STRING_SPLIT(@ClientStoreIds, ',');

    -- Iterate through the table of ClientStoreIds
    DECLARE @TempClientStoreId INT;

    DECLARE cur CURSOR FOR
    SELECT ClientStoreId FROM @TempData;

    OPEN cur;

    FETCH NEXT FROM cur INTO @TempClientStoreId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF EXISTS(SELECT 1 FROM SelectedStoresForCoupons WHERE Id = @TempClientStoreId)
        BEGIN
            UPDATE SelectedStoresForCoupons
            SET NewsId = @NewsId, StoreRouteId = @StoreRouteId
            WHERE Id = @TempClientStoreId;
        END
        ELSE
        BEGIN
            IF @TempClientStoreId = 0
            BEGIN
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid
                FROM ClientEnterprises CE
                INNER JOIN ClientEnterpriseRoutes CER 
                ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
                WHERE CER.IsRetailerlevel = 1;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteId AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(
                        NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId
                    )
                    VALUES (@NewsId, @StoreRouteId, GETDATE(), @TempClientStoreId, @EnterpriseId);
                END
            END
            ELSE
            BEGIN
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid
                FROM ClientStores CS
                INNER JOIN ClientEnterprises CE 
                ON CE.ClientEnterprisesid = CS.cliententerprisesid
                INNER JOIN ClientEnterpriseRoutes CER 
                ON CER.ClientEnterpriserouteId = CS.ClientEnterprisesrouteId
                WHERE CS.ClientStoreId = @TempClientStoreId AND CER.IsRetailerlevel = 0;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteId AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(
                        NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId
                    )
                    VALUES (@NewsId, @StoreRouteId, GETDATE(), @TempClientStoreId, @EnterpriseId);
                END
            END
        END

        FETCH NEXT FROM cur INTO @TempClientStoreId;
    END;

    CLOSE cur;
    DEALLOCATE cur;
----  stores end


    END   
END;


-------
-----INSERT TO Product s
ALTER PROCEDURE [dbo].[SaveSSNews_Products_UsingUPC] 
	@NewsId int,
	@ProductID int,
	@UPC varchar(20),
	@ProductName varchar(30)
AS
BEGIN

	SET NOCOUNT ON;


	IF(@ProductID = 0)
	BEGIN
		  SET @ProductId = (Select top 1 ProductId from product where productcode = @UPC)

		  IF(@ProductId = 0)
		  BEGIN
	     
			 -- If product upc is not found then insert and get productid
			   EXEC [dbo].[SaveProducts]  0, 
								@UPC, 
								1,
								1, 
								@ProductName, 
								@ProductName, 
								1, 
								1,
								1, 
								 1,
								 @UPC, 
								 1, 
								 1, 
								 1,
								 0,
								 'CompanyLogo.png',
								 '',
								 '',
								 1,
								 ''

				  SET @ProductId = (Select top 1 ProductId from product where productcode = @UPC)
		  END
	END




	IF EXISTS(SELECT *  FROM SSNews_Products WHERE ProductId = @ProductId AND NewsId = @NewsId)
	BEGIN
		UPDATE	SSNews_Products
		SET		
				NewsId = @NewsId,
				ProductId = @ProductId,
				CreatedDate = getdate()
		WHERE	ProductId = @ProductId  AND NewsId = @NewsId;
	END
	ELSE
	BEGIN
		INSERT INTO SSNews_Products(
		                NewsId, 
					    ProductId,
						CreatedDate 
				   )
				   VALUES(
				        @NewsId, 
					    @ProductId,
						GETDATE()
						)

		         
	END
END

----- 
ALTER PROCEDURE [dbo].[SaveSSNews_Products]
	-- Add the parameters for the stored procedure here
	@NewsId int,
	@ProductId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here


	IF EXISTS(SELECT *  FROM SSNews_Products WHERE ProductId = @ProductId AND NewsId = @NewsId)
	BEGIN
		UPDATE	SSNews_Products
		SET		
				NewsId = @NewsId,
				ProductId = @ProductId,
				CreatedDate = getdate()
		WHERE	ProductId = @ProductId  AND NewsId = @NewsId;
	END
	ELSE
	BEGIN
		INSERT INTO SSNews_Products(
		                NewsId, 
					    ProductId,
						CreatedDate 
				   )
				   VALUES(
				        @NewsId, 
					    @ProductId,
						GETDATE()
						)

		         
	END
END











------ UPDATE ---
 exec SaveCouponBasket
 @NewsID=0,
 @NewsCategoryID=4,
 @Title='test',
 @Details='',
 @ImagePath='',
 @ValidFromDate='',
 @ExpiresOn='',
 @SendNotification='',
 @CustomerID=9,
 @CreateUserID=1,
 @UpdateUserID=1,
 @PUICode='',
 @ProductId=1,
 @Amount=20,
 @DiscountAmount=0.2,
 @IsDiscountPercentage=false,
 @NCRPromotionCode='',
 @IsItStoreSpecific=false,
 @ManufacturerCouponId=9,
 @ProductQuantity=2,
 @UpSellProductId=2,
 @UpSellProductQuantity=1,
 @IsFeatured=false,
 @DeleteFlag=0,
 @IsItTargetSpecific=false,
 @OtherDetails='',
 @IsRecurring=false,
 @MfgShutOffDate='',
 @IsDealOftheWeek=false,
 @News_Id=0,
 @DepartmentId='1',
 @IsMajorDepartment=false,
 @StoreID='4,6',
 @PageNumber=2,
 @PdfFileName='',
 @Id=0,
 @StoreRouteId='',
 @ClientStoreId=4,
 @RecurringStartDate='',
 @RecurringEndDate='',
 @RecurringTypeId='',
 @ClubIds='',
 @GroupNames='',
 @ClientStoreIds=''
 


    -- Parameters
EXEC [dbo].[SaveCouponBasket] 
    @NewsID = 0, 
    @NewsCategoryID = 4, 
    @Title = 'Test Coupon', 
    @Details = 'Detailed description of the coupon', 
    @ImagePath = 'path/to/image.jpg', 
    @ValidFromDate = '2025-01-01 00:00:00', 
    @ExpiresOn = '2025-12-31 23:59:59', 
    @SendNotification = 1, 
    @CustomerID = 456, 
    @CreateUserID = 789, 
    @UpdateUserID = 789, 
    @PUICode = 'PUI123', 
    @ProductId = 101, 
    @Amount = 50.00, 
    @DiscountAmount = 5.00, 
    @IsDiscountPercentage = 1, 
    @NCRPromotionCode = 'PROMO123', 
    @IsItStoreSpecific = 0, 
    @ManufacturerCouponId = 10000000001, 
    @ProductQuantity = 10, 
    @UpSellProductId = 102, 
    @UpSellProductQuantity = 5, 
    @IsFeatured = 1, 
    @DeleteFlag = 0, 
    @IsItTargetSpecific = 1, 
    @OtherDetails = 'Extra details for the coupon', 
    @IsRecurring = 0, 
    @MfgShutOffDate = '2025-11-01 00:00:00', 
    @IsDealOftheWeek = 1, 
    @News_Id = NULL, 
    @DepartmentId = '1,2,3', 
    @IsMajorDepartment = 1, 
    @StoreID = '4', 
    @PageNumber = 1, 
    @PdfFileName = 'coupon.pdf', 
    @ClubId = 1, 
    @UserDetailId = 123456, 
    @ClubMemberId = 654321, 
    @Id = 0, 
    @StoreRouteId = 'route1', 
    @ClientStoreId = '4', 
    @RecurringStartDate = '2025-01-01 00:00:00', 
    @RecurringEndDate = '2025-12-31 23:59:59', 
    @RecurringTypeId = 1,
	@ClubIds='1,3',
	@GroupNames='$5 off your NEXT visit1,$5 off your NEXT visit1',
	@ClientStoreIds='4'
	
	SELECT * FROM SSNews
	select * from NewsCategories

	CREATE PROCEDURE [dbo].[SaveCouponBasket]
    -- Parameters
    @NewsID INT,
    @NewsCategoryID INT,
    @Title VARCHAR(200),
    @Details NVARCHAR(MAX),
    @ImagePath VARCHAR(200),
    @ValidFromDate DATETIME,
    @ExpiresOn DATETIME,
    @SendNotification BIT,
    @CustomerID INT,
    @CreateUserID INT,
    @UpdateUserID INT,
    @PUICode VARCHAR(25),
    @ProductId INT,
    @Amount MONEY,
    @DiscountAmount MONEY,
    @IsDiscountPercentage BIT,
    @NCRPromotionCode VARCHAR(50),
    @IsItStoreSpecific BIT,
    @ManufacturerCouponId BIGINT,
    @ProductQuantity INT,
    @UpSellProductId INT,
    @UpSellProductQuantity INT,
    @IsFeatured BIT,
    @DeleteFlag BIT,
    @IsItTargetSpecific BIT,
    @OtherDetails VARCHAR(300),
    @IsRecurring BIT,
    @MfgShutOffDate DATETIME,
    @IsDealOftheWeek BIT,
    @News_Id VARCHAR(10) OUT,
    @DepartmentId VARCHAR(MAX), -- New parameter for department IDs
    @IsMajorDepartment BIT,
    @StoreID VARCHAR(MAX), -- New parameter for Store IDs
    @PageNumber INT,
    @PdfFileName VARCHAR(200),
    @Id INT,
    @StoreRouteId VARCHAR(50),
    @ClientStoreId INT,
	@RecurringStartDate DATETIME,
	@RecurringEndDate DATETIME,
	@RecurringTypeId INT,
	 @ClubIds NVARCHAR(MAX),
    @GroupNames NVARCHAR(MAX),
	@ClientStoreIds NVARCHAR(MAX)
   

AS
BEGIN
    SET NOCOUNT ON;

    -- Check for deletion
    IF @DeleteFlag = 1
    BEGIN
        UPDATE SSNews
        SET NewsStatusID = 2
        WHERE NewsID = @NewsID;

        RETURN;
    END

    -- If record exists, update it
    IF EXISTS(SELECT 1 FROM SSNews WHERE NewsID = @NewsID)
    BEGIN
        UPDATE SSNews
        SET 
            NewsCategoryID = @NewsCategoryID,
            Title = @Title,
            Details = @Details,
            ImagePath = @ImagePath,
            ValidFromDate = @ValidFromDate,
            ExpiresOn = @ExpiresOn,
            SendNotification = @SendNotification,
            CustomerID = @CustomerID,
            UpdateUserID = @UpdateUserID,
            PUICode = @PUICode,
            Amount = @Amount,
            DiscountAmount = @DiscountAmount,
            IsDiscountPercentage = @IsDiscountPercentage,
            IsItStoreSpecific = @IsItStoreSpecific,
            IsFeatured = @IsFeatured,
            IsItTargetSpecific = @IsItTargetSpecific,
            OtherDetails = @OtherDetails,
            IsRecurring = @IsRecurring,
            UpdateDateTime = GETDATE(),
            ProductQuantity = @ProductQuantity,
            UpSellProductId = @UpSellProductId,
            UpSellProductQuantity = @UpSellProductQuantity,
            MfgShutOffDate = CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE @ExpiresOn END,
            IsDealOftheWeek = @IsDealOftheWeek
        WHERE NewsID = @NewsID;
    END
    ELSE
    BEGIN
        -- Insert a new record
        INSERT INTO SSNews(
            NewsCategoryID, Title, Details, ImagePath, ValidFromDate, ExpiresOn, SendNotification,
            CustomerID, CreateUserID, CreateDateTime, UpdateUserID, UpdateDateTime,
            PUICode, ProductId, Amount, DiscountAmount, IsDiscountPercentage, NCRPromotionCode,
            IsItStoreSpecific, ManufacturerCouponId, ProductQuantity, UpSellProductId,
            UpSellProductQuantity, IsFeatured, IsItTargetSpecific, OtherDetails, IsRecurring,
            MfgShutOffDate, IsDealOftheWeek
        )
        VALUES (
            @NewsCategoryID, @Title, @Details, @ImagePath, @ValidFromDate, @ExpiresOn, @SendNotification,
            @CustomerID, @CreateUserID, GETDATE(), @UpdateUserID, GETDATE(),
            @PUICode, @ProductId, @Amount, @DiscountAmount, @IsDiscountPercentage, @NCRPromotionCode,
            @IsItStoreSpecific, @ManufacturerCouponId, @ProductQuantity, @UpSellProductId,
            @UpSellProductQuantity, @IsFeatured, @IsItTargetSpecific, @OtherDetails, @IsRecurring,
            CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE @ExpiresOn END, @IsDealOftheWeek
        );

        -- Get the new record ID
        SET @News_Id = CAST(SCOPE_IDENTITY() AS VARCHAR(10));

		-- Insert into SSNews_Departments
    IF @DepartmentId IS NOT NULL
    BEGIN
        INSERT INTO SSNews_Departments(NewsId, DepartmentId, CreatedDate, IsMajorDepartment)
        SELECT @News_Id, Value, GETDATE(), @IsMajorDepartment
        FROM STRING_SPLIT(@DepartmentId, ',');
    END

	 -- Insert into WeeklyAddPDF
    IF @StoreID IS NOT NULL
    BEGIN
        INSERT INTO WeeklyAddPDF(StoreID, PageNumber, ValidFromDate, Expireson, PdfFileName, CreatedDate, ModifiedDate)
        SELECT Value, @PageNumber, @ValidFromDate, @ExpiresOn, @PdfFileName, GETDATE(), GETDATE()
        FROM STRING_SPLIT(@StoreID, ',');
    END

	 -- Insert into ClubUsers
    --IF @ClubId IS NOT NULL AND @UserDetailId IS NOT NULL
    --BEGIN
    --    INSERT INTO ClubUsers(ClubId, UserId, ClubMemberId, CreatedDate)
    --    VALUES (@ClubId, @UserDetailId, @ClubMemberId, GETDATE());
    --END


	  DECLARE @ClubIdTable TABLE (ClubId INT);
        DECLARE @GroupNameTable TABLE (GroupName NVARCHAR(100));

        -- Split ClubIds
        INSERT INTO @ClubIdTable (ClubId)
        SELECT value
        FROM STRING_SPLIT(@ClubIds, ',');

        -- Split GroupNames
        INSERT INTO @GroupNameTable (GroupName)
        SELECT value
        FROM STRING_SPLIT(@GroupNames, ',');

        -- Step 2: Process each combination of ClubId and GroupName
        DECLARE @CurrentClubId INT;
        DECLARE @CurrentGroupName NVARCHAR(100);
        DECLARE @CreatedDate DATETIME;
        DECLARE @UserId INT;

        -- Cursor for processing ClubIds and GroupNames
        DECLARE ClubCursor CURSOR FOR
        SELECT ClubId, GroupName
        FROM @ClubIdTable
        CROSS JOIN @GroupNameTable;

        OPEN ClubCursor;

        FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Retrieve CreatedDate from the clubs table
            SELECT TOP 1 
                @CreatedDate = CreatedDate
            FROM clubs
            WHERE ClubId = @CurrentClubId AND Name = @CurrentGroupName;

            -- If no matching record, skip to the next iteration
            IF @CreatedDate IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Retrieve UserId from clubUsers
            SELECT TOP 1
                @UserId = UserId
            FROM clubUsers
            WHERE ClubId = @CurrentClubId AND CreatedDate = @CreatedDate;

            -- If no matching user, skip to the next iteration
            IF @UserId IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Insert data into ClubUser table
            INSERT INTO ClubUsers(ClubId, UserId, ClubMemberId, CreatedDate)
            VALUES (@CurrentClubId, @UserId, NEWID(), GETDATE());

            FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
        END;

        CLOSE ClubCursor;
        DEALLOCATE ClubCursor;

	----  CludUser end

	---- save Recurrings start
IF @IsRecurring = 1
BEGIN
    IF EXISTS(SELECT * FROM SSNewsRecurrings WHERE NewsId = @NewsId)
    BEGIN
        UPDATE SSNewsRecurrings
           SET RecurringStartDate = @RecurringStartDate,
               RecurringEndDate = @RecurringEndDate,
               RecurringTypeId = @RecurringTypeId
           WHERE NewsId = @NewsId
    END
    ELSE
    BEGIN
        INSERT INTO SSNewsRecurrings 
        VALUES(@NewsId, @RecurringStartDate, @RecurringEndDate, @RecurringTypeId, GetDate())
    END
END

----   storestart
 --IF EXISTS(SELECT 1 FROM SelectedStoresForCoupons WHERE Id = @Id)
 --   BEGIN
 --       UPDATE SelectedStoresForCoupons
 --       SET NewsId = @NewsId, StoreRouteId = @StoreRouteId
 --       WHERE Id = @Id;
 --   END
 --   ELSE
 --   BEGIN
 --       DECLARE @StoreRouteIds VARCHAR(200), @EnterpriseId VARCHAR(200);

 --       IF @ClientStoreId = 0
 --       BEGIN
 --           SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid, @StoreRouteIds = CER.POSRouteId
 --           FROM ClientEnterprises CE
 --           INNER JOIN ClientEnterpriseRoutes CER ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
 --           WHERE CER.IsRetailerlevel = 1;

 --           IF NOT EXISTS(
 --               SELECT 1 FROM SelectedStoresForCoupons 
 --               WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId
 --           )
 --           BEGIN
 --               INSERT INTO SelectedStoresForCoupons(NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId)
 --               VALUES (@NewsId, @StoreRouteIds, GETDATE(), @ClientStoreId, @EnterpriseId);
 --           END
 --       END
 --       ELSE
 --       BEGIN
 --           SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid, @StoreRouteIds = CER.posrouteid
 --           FROM ClientStores CS
 --           INNER JOIN ClientEnterprises CE ON CE.ClientEnterprisesid = CS.cliententerprisesid
 --           INNER JOIN ClientEnterpriseRoutes CER ON CER.ClientEnterpriserouteId = CS.ClientEnterprisesrouteId
 --           WHERE CS.ClientStoreId = @ClientStoreId AND CER.IsRetailerlevel = 0;

 --           IF NOT EXISTS(
 --               SELECT 1 FROM SelectedStoresForCoupons 
 --               WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId
 --           )
 --           BEGIN
 --               INSERT INTO SelectedStoresForCoupons(NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId)
 --               VALUES (@NewsId, @StoreRouteIds, GETDATE(), @ClientStoreId, @EnterpriseId);
 --           END
 --       END
 --   END
-----  
 DECLARE @EnterpriseId VARCHAR(200);

    -- Table to hold split ClientStoreIds
    DECLARE @TempData TABLE (ClientStoreId INT);

    -- Split the ClientStoreIds string into rows
    INSERT INTO @TempData (ClientStoreId)
    SELECT value 
    FROM STRING_SPLIT(@ClientStoreIds, ',');

    -- Iterate through the table of ClientStoreIds
    DECLARE @TempClientStoreId INT;

    DECLARE cur CURSOR FOR
    SELECT ClientStoreId FROM @TempData;

    OPEN cur;

    FETCH NEXT FROM cur INTO @TempClientStoreId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF EXISTS(SELECT 1 FROM SelectedStoresForCoupons WHERE Id = @TempClientStoreId)
        BEGIN
            UPDATE SelectedStoresForCoupons
            SET NewsId = @NewsId, StoreRouteId = @StoreRouteId
            WHERE Id = @TempClientStoreId;
        END
        ELSE
        BEGIN
            IF @TempClientStoreId = 0
            BEGIN
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid
                FROM ClientEnterprises CE
                INNER JOIN ClientEnterpriseRoutes CER 
                ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
                WHERE CER.IsRetailerlevel = 1;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteId AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(
                        NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId
                    )
                    VALUES (@NewsId, @StoreRouteId, GETDATE(), @TempClientStoreId, @EnterpriseId);
                END
            END
            ELSE
            BEGIN
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid
                FROM ClientStores CS
                INNER JOIN ClientEnterprises CE 
                ON CE.ClientEnterprisesid = CS.cliententerprisesid
                INNER JOIN ClientEnterpriseRoutes CER 
                ON CER.ClientEnterpriserouteId = CS.ClientEnterprisesrouteId
                WHERE CS.ClientStoreId = @TempClientStoreId AND CER.IsRetailerlevel = 0;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteId AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(
                        NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId
                    )
                    VALUES (@NewsId, @StoreRouteId, GETDATE(), @TempClientStoreId, @EnterpriseId);
                END
            END
        END

        FETCH NEXT FROM cur INTO @TempClientStoreId;
    END;

    CLOSE cur;
    DEALLOCATE cur;
----  stores end


    END   
END;
------------------



 INSERT INTO ClubUser(ClubId,UserId,ClubMemberId,CreatedDate) 
 exec usp_InsertMultipleClubUsers @ClubIds='',@GroupNames=''

 exec PROC_CUSTOM_GET_ALL_SHOPPER_GROUPS @GroupID=99,@UserID =0


 CREATE PROCEDURE usp_InsertMultipleClubUsers
    @ClubIds NVARCHAR(MAX),
    @GroupNames NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Step 1: Split the ClubIds and GroupNames into table variables
        DECLARE @ClubIdTable TABLE (ClubId INT);
        DECLARE @GroupNameTable TABLE (GroupName NVARCHAR(100));

        -- Split ClubIds
        INSERT INTO @ClubIdTable (ClubId)
        SELECT value
        FROM STRING_SPLIT(@ClubIds, ',');

        -- Split GroupNames
        INSERT INTO @GroupNameTable (GroupName)
        SELECT value
        FROM STRING_SPLIT(@GroupNames, ',');

        -- Step 2: Process each combination of ClubId and GroupName
        DECLARE @CurrentClubId INT;
        DECLARE @CurrentGroupName NVARCHAR(100);
        DECLARE @CreatedDate DATETIME;
        DECLARE @UserId INT;

        -- Cursor for processing ClubIds and GroupNames
        DECLARE ClubCursor CURSOR FOR
        SELECT ClubId, GroupName
        FROM @ClubIdTable
        CROSS JOIN @GroupNameTable;

        OPEN ClubCursor;

        FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Retrieve CreatedDate from the clubs table
            SELECT TOP 1 
                @CreatedDate = CreatedDate
            FROM clubs
            WHERE ClubId = @CurrentClubId AND Name = @CurrentGroupName;

            -- If no matching record, skip to the next iteration
            IF @CreatedDate IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Retrieve UserId from clubUsers
            SELECT TOP 1
                @UserId = UserId
            FROM clubUsers
            WHERE ClubId = @CurrentClubId AND CreatedDate = @CreatedDate;

            -- If no matching user, skip to the next iteration
            IF @UserId IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Insert data into ClubUser table
            INSERT INTO ClubUsers(ClubId, UserId, ClubMemberId, CreatedDate)
            VALUES (@CurrentClubId, @UserId, NEWID(), GETDATE());

            FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
        END;

        CLOSE ClubCursor;
        DEALLOCATE ClubCursor;

        PRINT 'Data inserted successfully for all valid combinations.';
    END TRY
    BEGIN CATCH
        -- Handle errors
        PRINT 'An error occurred.';
        PRINT ERROR_MESSAGE();
    END CATCH
END;


select * from MFG_Coupons
SELECT * FROM SSNews
EXEC dbo.GetStoreCouponsToUpdateForNCR
EXEC dbo.GetStoreEnterPrises
EXEC dbo.GetNewsCategories;
		
select * from SelectedProductIdForWeeklySpecials

SELECT *  FROM SelectedStoresForCoupons




 --- EDIT PROC
 ALTER PROCEDURE [dbo].[SaveCouponBasket]
    -- Parameters
    @NewsID INT,
    @NewsCategoryID INT,
    @Title VARCHAR(200),
    @Details NVARCHAR(MAX),
    @ImagePath VARCHAR(200),
    @ValidFromDate DATETIME,
    @ExpiresOn DATETIME,
    @SendNotification BIT,
    @CustomerID INT,
    @CreateUserID INT,
    @UpdateUserID INT,
    @PUICode VARCHAR(25),
    @ProductId INT,
    @Amount MONEY,
    @DiscountAmount MONEY,
    @IsDiscountPercentage BIT,
    @NCRPromotionCode VARCHAR(50),
    @IsItStoreSpecific BIT,
    @ManufacturerCouponId BIGINT,
    @ProductQuantity INT,
    @UpSellProductId INT,
    @UpSellProductQuantity INT,
    @IsFeatured BIT,
    @DeleteFlag BIT,
    @IsItTargetSpecific BIT,
    @OtherDetails VARCHAR(300),
    @IsRecurring BIT,
    @MfgShutOffDate DATETIME,
    @IsDealOftheWeek BIT,
    @News_Id VARCHAR(10) OUT,
    @DepartmentId VARCHAR(MAX),
    @IsMajorDepartment BIT,
    @StoreID VARCHAR(MAX),
    @PageNumber INT,
    @PdfFileName VARCHAR(200),
    @ClubId INT,
    @UserDetailId INT,
    @ClubMemberId INT,
    @Id INT,
    @StoreRouteId VARCHAR(50),
    @ClientStoreId INT,
    @RecurringStartDate DATETIME,
    @RecurringEndDate DATETIME,
    @RecurringTypeId INT,
	@ClubIds NVARCHAR(MAX),
    @GroupNames NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check for deletion
    IF @DeleteFlag = 1
    BEGIN
        UPDATE SSNews
        SET NewsStatusID = 2
        WHERE NewsID = @NewsID;
        RETURN;
    END

    -- If record exists, update it
    IF EXISTS(SELECT 1 FROM SSNews WHERE NewsID = @NewsID)
    BEGIN
        UPDATE SSNews
        SET 
            NewsCategoryID = @NewsCategoryID,
            Title = @Title,
            Details = @Details,
            ImagePath = @ImagePath,
            ValidFromDate = @ValidFromDate,
            ExpiresOn = @ExpiresOn,
            SendNotification = @SendNotification,
            CustomerID = @CustomerID,
            UpdateUserID = @UpdateUserID,
            PUICode = @PUICode,
            Amount = @Amount,
            DiscountAmount = @DiscountAmount,
            IsDiscountPercentage = @IsDiscountPercentage,
            IsItStoreSpecific = @IsItStoreSpecific,
            IsFeatured = @IsFeatured,
            IsItTargetSpecific = @IsItTargetSpecific,
            OtherDetails = @OtherDetails,
            IsRecurring = @IsRecurring,
            UpdateDateTime = GETDATE(),
            ProductQuantity = @ProductQuantity,
            UpSellProductId = @UpSellProductId,
            UpSellProductQuantity = @UpSellProductQuantity,
            MfgShutOffDate = CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE @ExpiresOn END,
            IsDealOftheWeek = @IsDealOftheWeek
        WHERE NewsID = @NewsID;
    END
    ELSE
    BEGIN
        -- Insert a new record
        INSERT INTO SSNews(
            NewsCategoryID, Title, Details, ImagePath, ValidFromDate, ExpiresOn, SendNotification,
            CustomerID, CreateUserID, CreateDateTime, UpdateUserID, UpdateDateTime,
            PUICode, ProductId, Amount, DiscountAmount, IsDiscountPercentage, NCRPromotionCode,
            IsItStoreSpecific, ManufacturerCouponId, ProductQuantity, UpSellProductId,
            UpSellProductQuantity, IsFeatured, IsItTargetSpecific, OtherDetails, IsRecurring,
            MfgShutOffDate, IsDealOftheWeek
        )
        VALUES (
            @NewsCategoryID, @Title, @Details, @ImagePath, @ValidFromDate, @ExpiresOn, @SendNotification,
            @CustomerID, @CreateUserID, GETDATE(), @UpdateUserID, GETDATE(),
            @PUICode, @ProductId, @Amount, @DiscountAmount, @IsDiscountPercentage, @NCRPromotionCode,
            @IsItStoreSpecific, @ManufacturerCouponId, @ProductQuantity, @UpSellProductId,
            @UpSellProductQuantity, @IsFeatured, @IsItTargetSpecific, @OtherDetails, @IsRecurring,
            CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE @ExpiresOn END, @IsDealOftheWeek
        );

        -- Get the new record ID
        SET @News_Id = CAST(SCOPE_IDENTITY() AS VARCHAR(10));

        -- Insert into SSNews_Departments
        IF @DepartmentId IS NOT NULL
        BEGIN
            INSERT INTO SSNews_Departments(NewsId, DepartmentId, CreatedDate, IsMajorDepartment)
            SELECT @News_Id, Value, GETDATE(), @IsMajorDepartment
            FROM STRING_SPLIT(@DepartmentId, ',');
        END

        -- Insert into WeeklyAddPDF
        IF @StoreID IS NOT NULL
        BEGIN
            INSERT INTO WeeklyAddPDF(StoreID, PageNumber, ValidFromDate, Expireson, PdfFileName, CreatedDate, ModifiedDate)
            SELECT Value, @PageNumber, @ValidFromDate, @ExpiresOn, @PdfFileName, GETDATE(), GETDATE()
            FROM STRING_SPLIT(@StoreID, ',');
        END

        -- Insert into ClubUsers
        --IF @ClubId IS NOT NULL AND @UserDetailId IS NOT NULL
        --BEGIN
        --    INSERT INTO ClubUsers(ClubId, UserId, ClubMemberId, CreatedDate)
        --    VALUES (@ClubId, @UserDetailId, @ClubMemberId, GETDATE());
        --END

		 DECLARE @ClubIdTable TABLE (ClubId INT);
        DECLARE @GroupNameTable TABLE (GroupName NVARCHAR(100));

        -- Split ClubIds
        INSERT INTO @ClubIdTable (ClubId)
        SELECT value
        FROM STRING_SPLIT(@ClubIds, ',');

        -- Split GroupNames
        INSERT INTO @GroupNameTable (GroupName)
        SELECT value
        FROM STRING_SPLIT(@GroupNames, ',');

        -- Step 2: Process each combination of ClubId and GroupName
        DECLARE @CurrentClubId INT;
        DECLARE @CurrentGroupName NVARCHAR(100);
        DECLARE @CreatedDate DATETIME;
        DECLARE @UserId INT;

        -- Cursor for processing ClubIds and GroupNames
        DECLARE ClubCursor CURSOR FOR
        SELECT ClubId, GroupName
        FROM @ClubIdTable
        CROSS JOIN @GroupNameTable;

        OPEN ClubCursor;

        FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Retrieve CreatedDate from the clubs table
            SELECT TOP 1 
                @CreatedDate = CreatedDate
            FROM clubs
            WHERE ClubId = @CurrentClubId AND Name = @CurrentGroupName;

            -- If no matching record, skip to the next iteration
            IF @CreatedDate IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Retrieve UserId from clubUsers
            SELECT TOP 1
                @UserId = UserId
            FROM clubUsers
            WHERE ClubId = @CurrentClubId AND CreatedDate = @CreatedDate;

            -- If no matching user, skip to the next iteration
            IF @UserId IS NULL
            BEGIN
                FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
                CONTINUE;
            END

            -- Insert data into ClubUser table
            INSERT INTO ClubUsers (ClubId, UserId, ClubMemberId, CreatedDate)
            VALUES (@CurrentClubId, @UserId, NEWID(), GETDATE());

            FETCH NEXT FROM ClubCursor INTO @CurrentClubId, @CurrentGroupName;
        END;

        CLOSE ClubCursor;
        DEALLOCATE ClubCursor;

        -- Handle Recurrings
        IF @IsRecurring = 1
        BEGIN
            IF EXISTS(SELECT * FROM SSNewsRecurrings WHERE NewsId = @NewsId)
            BEGIN
                UPDATE SSNewsRecurrings
                SET RecurringStartDate = @RecurringStartDate,
                    RecurringEndDate = @RecurringEndDate,
                    RecurringTypeId = @RecurringTypeId
                WHERE NewsId = @NewsId;
            END
            ELSE
            BEGIN
                INSERT INTO SSNewsRecurrings 
                VALUES(@NewsId, @RecurringStartDate, @RecurringEndDate, @RecurringTypeId, GetDate());
            END
        END

        -- Handle SelectedStoresForCoupons
        IF EXISTS(SELECT 1 FROM SelectedStoresForCoupons WHERE Id = @Id)
        BEGIN
            UPDATE SelectedStoresForCoupons
            SET NewsId = @NewsId, StoreRouteId = @StoreRouteId
            WHERE Id = @Id;
        END
        ELSE
        BEGIN
            DECLARE @StoreRouteIds VARCHAR(200), @EnterpriseId VARCHAR(200);

            IF @ClientStoreId = 0
            BEGIN
                -- Get Enterprise and StoreRouteId
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid, @StoreRouteIds = CER.POSRouteId
                FROM ClientEnterprises CE
                INNER JOIN ClientEnterpriseRoutes CER ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
                WHERE CER.IsRetailerlevel = 1;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId)
                    VALUES (@NewsId, @StoreRouteIds, GETDATE(), @ClientStoreId, @EnterpriseId);
                END
            END
            ELSE
            BEGIN
                -- Get Store and Route details for ClientStoreId
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid, @StoreRouteIds = CER.posrouteid
                FROM ClientStores CS
                INNER JOIN ClientEnterprises CE ON CE.ClientEnterprisesid = CS.cliententerprisesid
                INNER JOIN ClientEnterpriseRoutes CER ON CER.ClientEnterpriserouteId = CS.ClientEnterprisesrouteId
                WHERE CS.ClientStoreId = @ClientStoreId AND CER.IsRetailerlevel = 0;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId)
                    VALUES (@NewsId, @StoreRouteIds, GETDATE(), @ClientStoreId, @EnterpriseId);
                END
            END
        END
    END
END;


select * from DISTRIBUTOR_COUPONS;

select * from BasketCoupons
select * from BasketData
select * from WEEKLYADDPDF
select * from SSNews;
select * from ClubUsers;

select * from SSNews_Departments;
select * from ProductCategories;
select * from product;
select * from MFG_COUPONS;


EXEC SaveCouponBasket
 @NewsID = 0,
    @NewsCategoryID = 3,
    @Title = 'Test special',
    @Details = 'Discount on selected items',
    @ImagePath = 'images/offer.jpg',
    @ValidFromDate = '2025-01-01',
    @ExpiresOn = '2025-02-01',
	@SendNotification = 1,
	 @CustomerID = 1,
	  @CreateUserID = 1,
	   @UpdateUserID = 1,
	   @PUICode = '',
	   @ProductId = NULL,
	   @Amount=10,
	   @DiscountAmount=0.9,
	   @IsDiscountPercentage=1,
	   @NCRPromotionCode='',
	   @IsItStoreSpecific=1,
	   @ManufacturerCouponId=NULL,
	   @ProductQuantity=1,
	   @UpSellProductId=1,
	   @UpSellProductQuantity=1,
	   @IsFeatured=1,
	   @DeleteFlag=0,
	   @IsItTargetSpecific=1,
	   @OtherDetails='',
	   @IsRecurring=0,
	   @MfgShutOffDate='2025-02-01',
	   @IsDealOftheWeek=0,
	   @News_Id=0,
	   @DepartmentId=4,
	   @IsMajorDepartment=1,
	   @StoreID='4,6',
	   @PageNumber=1,
	   @PdfFileName='PdfExample',
	   @ClubId=228,
	   @UserDetailId=3,
	   @ClubMemberId=3,
	   @Id=0,
	   @StoreRouteId='',
	   @ClientStoreId=0,
	   @RecurringStartDate='',
	   @RecurringEndDate='',
	   @RecurringTypeId=0

	   EXEC [dbo].[SaveCouponBasket]
    @NewsID = 0,
    @NewsCategoryID = 4,
    @Title = 'demo coupon',
    @Details = 'Sample Test details for the coupon basket',
    @ImagePath = 'sample/path/to/image.jpg',
    @ValidFromDate = '2025-01-01 00:00:00',
    @ExpiresOn = '2025-12-31 23:59:59',
    @SendNotification = 1,
    @CustomerID = 12345,
    @CreateUserID = 67890,
    @UpdateUserID = 67890,
    @PUICode = 'PUICODE12345',
    @ProductId = 456,
    @Amount = 99.99,
    @DiscountAmount = 10.00,
    @IsDiscountPercentage = 0,
    @NCRPromotionCode = 'PROMO123',
    @IsItStoreSpecific = 1,
    @ManufacturerCouponId = 987654321,
    @ProductQuantity = 5,
    @UpSellProductId = 123,
    @UpSellProductQuantity = 2,
    @IsFeatured = 1,
    @DeleteFlag = 0,
    @IsItTargetSpecific = 1,
    @OtherDetails = 'Additional details for the coupon',
    @IsRecurring = 0,
    @MfgShutOffDate = '2025-06-01 00:00:00',
    @IsDealOftheWeek = 1,
    @News_Id = 0,
    @DepartmentId = 0,
    @IsMajorDepartment = 0,
    @StoreID = '101,102,103', -- Comma-separated list of StoreIDs
    @PageNumber = 5,
    @PdfFileName = 'WeeklyAd.pdf',
    @ClubId = 3,
    @UserDetailId = 789,
    @ClubMemberId = 456,
    @Id = 1,
    @NewsId = '',
    @StoreRouteId = '',
    @ClientStoreId = 0,
	@RecurringStartDate='2025-06-01',
	@RecurringEndDate='2025-06-01',
	@RecurringTypeId=1

-- To capture the output parameter `@News_Id`, declare a variable and use it as follows:
DECLARE @OutputNewsId VARCHAR(10);

EXEC [dbo].[SaveCouponBasket]
    @NewsID = 0,
    @NewsCategoryID = 2,
    @Title = 'Test coupon details',
    @Details = 'Sample details for the coupon basket',
    @ImagePath = 'sample/path/to/image.jpg',
    @ValidFromDate = '2025-01-01 00:00:00',
    @ExpiresOn = '2025-12-31 23:59:59',
    @SendNotification = 1,
    @CustomerID = 12345,
    @CreateUserID = 67890,
    @UpdateUserID = 67890,
    @PUICode = 'PUICODE12345',
    @ProductId = 456,
    @Amount = 99.99,
    @DiscountAmount = 10.00,
    @IsDiscountPercentage = 0,
    @NCRPromotionCode = 'PROMO123',
    @IsItStoreSpecific = 1,
    @ManufacturerCouponId = 987654321,
    @ProductQuantity = 5,
    @UpSellProductId = 123,
    @UpSellProductQuantity = 2,
    @IsFeatured = 1,
    @DeleteFlag = 0,
    @IsItTargetSpecific = 1,
    @OtherDetails = 'Additional details for the coupon',
    @IsRecurring = 0,
    @MfgShutOffDate = '2025-06-01 00:00:00',
    @IsDealOftheWeek = 1,
    @News_Id =0,
    @DepartmentId = 10,
    @IsMajorDepartment = 1,
    @StoreID = '101,102,103', -- Comma-separated list of StoreIDs
    @PageNumber = 5,
    @PdfFileName = 'WeeklyAd.pdf',
    @ClubId = 3,
    @UserDetailId = 789,
    @ClubMemberId = 456,
    @Id = 1,
    @NewsId = 1,
    @StoreRouteId = 'LTE76HR8BTRLY7CS9VTJE7JXES',
    @ClientStoreId = 0;

PRINT @OutputNewsId;

	
select * from  SelectedStoresForCoupons





-----MULTIPLES STOREs


CREATE PROCEDURE [dbo].[HandleSelectedStoresForCoupons]
    @NewsId INT,
    @ClientStoreIds NVARCHAR(MAX), -- Comma-separated string of ClientStoreIds
    @StoreRouteId VARCHAR(200) -- Single StoreRouteId
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EnterpriseId VARCHAR(200);

    -- Table to hold split ClientStoreIds
    DECLARE @TempData TABLE (ClientStoreId INT);

    -- Split the ClientStoreIds string into rows
    INSERT INTO @TempData (ClientStoreId)
    SELECT value 
    FROM STRING_SPLIT(@ClientStoreIds, ',');

    -- Iterate through the table of ClientStoreIds
    DECLARE @TempClientStoreId INT;

    DECLARE cur CURSOR FOR
    SELECT ClientStoreId FROM @TempData;

    OPEN cur;

    FETCH NEXT FROM cur INTO @TempClientStoreId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF EXISTS(SELECT 1 FROM SelectedStoresForCoupons WHERE Id = @TempClientStoreId)
        BEGIN
            UPDATE SelectedStoresForCoupons
            SET NewsId = @NewsId, StoreRouteId = @StoreRouteId
            WHERE Id = @TempClientStoreId;
        END
        ELSE
        BEGIN
            IF @TempClientStoreId = 0
            BEGIN
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid
                FROM ClientEnterprises CE
                INNER JOIN ClientEnterpriseRoutes CER 
                ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
                WHERE CER.IsRetailerlevel = 1;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteId AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(
                        NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId
                    )
                    VALUES (@NewsId, @StoreRouteId, GETDATE(), @TempClientStoreId, @EnterpriseId);
                END
            END
            ELSE
            BEGIN
                SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid
                FROM ClientStores CS
                INNER JOIN ClientEnterprises CE 
                ON CE.ClientEnterprisesid = CS.cliententerprisesid
                INNER JOIN ClientEnterpriseRoutes CER 
                ON CER.ClientEnterpriserouteId = CS.ClientEnterprisesrouteId
                WHERE CS.ClientStoreId = @TempClientStoreId AND CER.IsRetailerlevel = 0;

                IF NOT EXISTS(
                    SELECT 1 FROM SelectedStoresForCoupons 
                    WHERE NewsId = @NewsId AND StoreRouteId = @StoreRouteId AND EnterpriseId = @EnterpriseId
                )
                BEGIN
                    INSERT INTO SelectedStoresForCoupons(
                        NewsId, StoreRouteId, CreatedDate, ClientStoreId, EnterpriseId
                    )
                    VALUES (@NewsId, @StoreRouteId, GETDATE(), @TempClientStoreId, @EnterpriseId);
                END
            END
        END

        FETCH NEXT FROM cur INTO @TempClientStoreId;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO





















    
   
   
   
   select * from SSNews
    

ALTER PROCEDURE [dbo].[SaveCouponBasket]
    -- Parameters for the stored procedure
    @NewsID int,
    @NewsCategoryID int,
    @Title varchar(200),
    @Details nvarchar(max),
    @ImagePath varchar(200),
    @ValidFromDate datetime,
    @ExpiresOn datetime,
    @SendNotification bit,
    @CustomerID int,
    @CreateUserID int,
    @UpdateUserID int,
    @PUICode varchar(25),
    @ProductId int,
    @Amount money,
    @DiscountAmount money,
    @IsDiscountPercentage bit,
    @NCRPromotionCode varchar(50),
    @IsItStoreSpecific bit,
    @ManufacturerCouponId bigint,
    @ProductQuantity int,
    @UpSellProductId int,
    @UpSellProductQuantity int,
    @IsFeatured bit,
    @DeleteFlag bit,
    @IsItTargetSpecific bit,
    @OtherDetails varchar(300),
    @IsRecurring bit,
    @MfgShutOffDate datetime,
    @IsDealOftheWeek bit,
    @News_Id varchar(10) out,
    @DepartmentId int, -- New parameter for DepartmentId
    @IsMajorDepartment bit, -- New parameter for IsMajorDepartment
    @StoreID varchar(50), -- New parameter for StoreID (comma-separated list of StoreIDs)
    @PageNumber int, -- New parameter for PageNumber
    @PdfFileName varchar(200), -- New parameter for PdfFileName
    @ClubId int, -- New parameter for ClubId
    @UserDetailId int, -- New parameter for UserDetailId
    @ClubMemberId int, -- New parameter for ClubMemberId
	@Id int,
	@NewsId int,
	@StoreRouteId varchar(50),
	@ClientStoreId int
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Plogo varchar(50);

    IF (CAST(@DeleteFlag AS bigint) = 1)
    BEGIN
        UPDATE SSNews
        SET NewsStatusID = 2
        WHERE NewsID = @NewsID;
    END
    ELSE IF EXISTS(SELECT * FROM SSNews WHERE NewsID = @NewsID)
    BEGIN
        UPDATE SSNews
        SET 
            NewsCategoryID = @NewsCategoryID,
            Title = @Title,
            Details = @Details,
            ImagePath = @ImagePath,
            ValidFromDate = DATEADD(hour, 0, @ValidFromDate),
            ExpiresOn = DATEADD(hour, 0, @ExpiresOn),
            SendNotification = @SendNotification,
            CustomerID = @CustomerID,
            UpdateUserID = @UpdateUserID,
            PUICode = @PUICode,
            Amount = @Amount,
            DiscountAmount = @DiscountAmount,
            IsDiscountPercentage = @IsDiscountPercentage,
            IsItStoreSpecific = @IsItStoreSpecific,
            IsFeatured = @IsFeatured,
            IsItTargetSpecific = @IsItTargetSpecific,
            OtherDetails = @OtherDetails,
            IsRecurring = @IsRecurring,
            UpdateDateTime = GETDATE(),
            ProductQuantity = @ProductQuantity,
            UpSellProductId = @UpSellProductId,
            UpSellProductQuantity = @UpSellProductQuantity,
            MfgShutOffDate = CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE DATEADD(hour, 0, @ExpiresOn) END,
            IsDealOftheWeek = @IsDealOftheWeek
        WHERE NewsID = @NewsID;
    END
    ELSE
    BEGIN
        IF (@NewsCategoryID = 4 AND @ManufacturerCouponId > 0 AND @NewsId = 0)
        BEGIN
            IF EXISTS(SELECT NewsId FROM SSNews WHERE ManufacturerCouponId = @ManufacturerCouponId)
            BEGIN
                SET @News_Id = -10001;
                RETURN @News_Id;
            END
        END

        INSERT INTO SSNews(
            NewsCategoryID, 
            Title,
            Details, 
            ImagePath, 
            ValidFromDate, 
            ExpiresOn, 
            SendNotification,
            CustomerID, 
            CreateUserID,
            CreateDateTime, 
            UpdateUserID, 
            UpdateDateTime,
            CouponuniqueId,
            PUICode,
            ProductId,
            Amount,
            CouponCode,
            DiscountAmount,
            IsDiscountPercentage,
            NCRPromotionCode,
            NCRPromotionCreatedDate,
            IsItStoreSpecific,
            ManufacturerCouponId,
            ProductQuantity,
            UpSellProductId,
            UpSellProductQuantity,
            IsFeatured,
            IsItTargetSpecific,
            OtherDetails,
            IsRecurring,
            MfgShutOffDate,
            IsDealOftheWeek
        )
        VALUES(
            @NewsCategoryID, 
            @Title,
            @Details, 
            @ImagePath, 
            DATEADD(hour, 0, @ValidFromDate),
            DATEADD(hour, 0, @ExpiresOn),
            @SendNotification,
            @CustomerID, 
            @CreateUserID,
            GETDATE(), 
            @UpdateUserID, 
            GETDATE(),
            NEWID(),
            @PUICode,
            CASE 
                WHEN @NewsCategoryID = 3 THEN 1
                WHEN @NewsCategoryID = 4 THEN 0
                WHEN @NewsCategoryID = 5 THEN 3
                WHEN @NewsCategoryID = 6 THEN 2
                ELSE @ProductId 
            END,
            @Amount,
            (SELECT CAST(RAND() * 10000000000 AS bigint)),
            @DiscountAmount,
            @IsDiscountPercentage,
            @NCRPromotionCode,
            GETDATE(),
            @IsItStoreSpecific,
            @ManufacturerCouponId,
            @ProductQuantity,
            @UpSellProductId,
            @UpSellProductQuantity,
            @IsFeatured,
            @IsItTargetSpecific,
            @OtherDetails,
            @IsRecurring,
            CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE DATEADD(hour, 0, @ExpiresOn) END,
            @IsDealOftheWeek
        );

        SET @News_Id = SCOPE_IDENTITY();

        -- Insert into SSNews_Departments
 INSERT INTO @DepartmentIds (DepartmentId)
SELECT value 
FROM STRING_SPLIT(@DepartmentId, ',')

-- Insert into SSNews_Departments for each DepartmentId in the table variable
INSERT INTO SSNews_Departments(
    NewsId, 
    DepartmentId,
    CreatedDate,
    IsMajorDepartment
)
SELECT 
    @News_Id, 
    DepartmentId,
    GETDATE(),
    @IsMajorDepartment
FROM @DepartmentIds
WHERE DepartmentId <> 0; 
        -- Insert into WeeklyAddPDF for each StoreID in the comma-separated list
        DECLARE @StoreIDList TABLE (StoreID INT);

        -- Split the comma-separated StoreID list and insert into the table variable
        INSERT INTO @StoreIDList (StoreID)
        SELECT value
        FROM STRING_SPLIT(@StoreID, ',');

        DECLARE @CurrentStoreID INT;

        DECLARE StoreCursor CURSOR FOR
        SELECT StoreID FROM @StoreIDList;

        OPEN StoreCursor;
        FETCH NEXT FROM StoreCursor INTO @CurrentStoreID;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Insert data into WeeklyAddPDF table for each StoreID
            INSERT INTO WeeklyAddPDF(
                StoreID,
                PageNumber,
                ValidFromDate,
                Expireson,
                PdfFileName,
                CreatedDate,
                ModifiedDate 
            )
            VALUES(
                @CurrentStoreID,
                @PageNumber,
                @ValidFromDate,
                @ExpiresOn,
                @PdfFileName,
                GETDATE(),
                GETDATE()
            );

            FETCH NEXT FROM StoreCursor INTO @CurrentStoreID;
        END

        CLOSE StoreCursor;
        DEALLOCATE StoreCursor;

        -- Insert into ClubUsers
        INSERT INTO ClubUsers(
            ClubId,
            UserId,
            ClubMemberId,
            CreatedDate
        )
        VALUES(
            @ClubId,
			3469,
			3,
			
            --@UserDetailId,
            --@ClubMemberId,
            GETDATE()
        );


		---- insert into selected coupons stores
		IF EXISTS(SELECT *  FROM SelectedStoresForCoupons WHERE Id = @Id)
	BEGIN
		UPDATE	SelectedStoresForCoupons
		SET		
		        NewsId =@NewsId,
				StoreRouteId =@StoreRouteId
		WHERE	Id = @Id
	END
	ELSE
	BEGIN

	Declare @StoreRouteIds As Varchar(200);
	Declare @EnterpriseId varchar(200);

	-- @ClientStoreId is 0 means that is Non specific strore
	IF( @ClientStoreId = 0)
	BEGIN

	--SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid,
 --          @StoreRouteIds = CER.POSRouteId 
	--FROM ClientStores as CS
	--INNER JOIN ClientEnterprises AS CE ON CE.ClientEnterprisesid = CS.cliententerprisesid
	--INNER JOIN ClientEnterpriseRoutes AS CER ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
	--WHERE CER.IsRetailerlevel = 1
	----CS.ClientStoreId = @ClientStoreId

	SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid,
           @StoreRouteIds = CER.POSRouteId 
	FROM ClientEnterprises as CE
	INNER JOIN ClientEnterpriseRoutes AS CER ON CER.ClientEnterprisesid = CE.ClientEnterprisesid
	WHERE CER.IsRetailerlevel = 1

	IF NOT EXISTS( SELECT NewsId FROM SelectedStoresForCoupons Where NewsId =  @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId )
		BEGIN


		INSERT INTO SelectedStoresForCoupons(
					    NewsId,
						StoreRouteId, 
						CreatedDate,
						ClientStoreId,
						EnterpriseId
				   )
				   VALUES(
				        @NewsId,
						@StoreRouteIds, 
						GETDATE(),
						@ClientStoreId,
						@EnterpriseId
						)
		END
	END
	ELSE 
	BEGIN

	-- Store Specific level

	SELECT DISTINCT @EnterpriseId = CE.POSEnterpriseid,
           @StoreRouteIds = CER.posrouteid
	FROM ClientStores as CS
	INNER JOIN ClientEnterprises AS CE ON CE.ClientEnterprisesid = CS.cliententerprisesid
	INNER JOIN ClientEnterpriseRoutes AS CER ON CER.ClientEnterpriserouteid = CS.ClientEnterprisesrouteId
	WHERE
	CS.ClientStoreId = @ClientStoreId
    AND CER.IsRetailerlevel = 0

	IF NOT EXISTS( SELECT NewsId FROM SelectedStoresForCoupons Where NewsId =  @NewsId AND StoreRouteId = @StoreRouteIds AND EnterpriseId = @EnterpriseId )
		BEGIN

		INSERT INTO SelectedStoresForCoupons(
					    NewsId,
						StoreRouteId, 
						CreatedDate,
						ClientStoreId,
						EnterpriseId
				   )
				   VALUES(
				        @NewsId,
						@StoreRouteIds, 
						GETDATE(),
						@ClientStoreId,
						@EnterpriseId
						)
			END

	END


		         
	END

    END
END;


















CREATE proc BasketDealsCoupon (
 @Title varchar(225),
 @Details varchar(225),
 @OtherDetails varchar(225),
 @MinPurchaseAmount money,
 @CustomerSavings money,
 @OfferValidFrom datetime,
 @OfferValidTo datetime,
 @ImagePath varchar(max),

@Departments varchar(max),
@Groups varchar(max),
@Stores varchar(max),
@StoresWeeklyAdds varchar(max),
@IsStoreSpecific bit,
@IsDepartmentSpecific bit,
@IsRecurring bit,
@IsFeatured bit

)

AS
	BEGIN 

	  ---- HOW  TO CREATE COUPONS WITH ABOVE DETAILS
	  

	  	INSERT INTO SSNews(
		                NewsCategoryID, 
					    Title,
						Details, 
						ImagePath, 
						ValidFromDate, 
						ExpiresOn, 
						SendNotification,
						CustomerID, 
						CreateUserID,
						CreateDateTime, 
						UpdateUserID, 
						UpdateDateTime,
						CouponuniqueId,
						PUICode,
						ProductId,
						Amount,
						CouponCode,
						DiscountAmount,
						IsDiscountPercentage,
						NCRPromotionCode,
						NCRPromotionCreatedDate,
						IsItStoreSpecific,
						ManufacturerCouponId,
						ProductQuantity,
						UpSellProductId,
						UpSellProductQuantity,
						IsFeatured,
						IsItTargetSpecific,
						OtherDetails,
						IsRecurring,
						MfgShutOffDate,
						IsDealOftheWeek
				   )
				   VALUES(
				        @NewsCategoryID, 
					    @Title,
						@Details, 
						@ImagePath, 
						DATEADD (hour , 0 , @ValidFromDate),
						DATEADD (hour , 0 , @ExpiresOn),
						--@ValidFromDate, 
						--@ExpiresOn, 
						@SendNotification,
						@CustomerID, 
						@CreateUserID,
						GETDATE(), 
						@UpdateUserID, 
						GETDATE(),
						NEWID(),
						@PUICode,
						--@ProductId,
						  CASE 
							WHEN @NewsCategoryID = 3  THEN 1  --Basket Product
							WHEN @NewsCategoryID = 4 THEN 0  -- Manufacturer Product
							WHEN @NewsCategoryID = 5 THEN 3  -- Group Coupon
							WHEN @NewsCategoryID = 6 THEN 2  -- Group Basket
							ELSE @ProductId 
						END ,
						@Amount,
						(SELECT CAST(RAND() * 10000000000 AS bigint)),
						@DiscountAmount,
						@IsDiscountPercentage,
						@NCRPromotionCode,
						GETDATE(),
						@IsItStoreSpecific,
						@ManufacturerCouponId,
						@ProductQuantity,
						@UpSellProductId,
						@UpSellProductQuantity,
						@IsFeatured,
						@IsItTargetSpecific,
						--CASE WHEN @NewsCategoryID = 4 THEN 1 ELSE @IsItTargetSpecific END, --TBD remove for go live
						@OtherDetails,
						@IsRecurring,
						CASE WHEN @NewsCategoryID = 4 THEN @MfgShutOffDate ELSE DATEADD (hour , 0 , @ExpiresOn) END,
						@IsDealOftheWeek
						)
					IF EXISTS(SELECT *  FROM SSNews_Departments WHERE DepartmentId = @DepartmentId AND NewsId = @NewsId)
	BEGIN
		UPDATE	SSNews_Departments
		SET		
				NewsId = @NewsId,
				DepartmentId = @DepartmentId,
				CreatedDate = getdate()
		WHERE	DepartmentId = @DepartmentId  AND NewsId = @NewsId;
	END
	ELSE
	BEGIN
		INSERT INTO SSNews_Departments(
		                NewsId, 
					    DepartmentId,
						CreatedDate ,
						IsMajorDepartment
				   )
				   VALUES(
				        @NewsId, 
					    @DepartmentId,
						GETDATE(),
						@IsMajorDepartment
						)
			IF EXISTS(SELECT *  FROM SSNews_Products WHERE ProductId = @ProductId AND NewsId = @NewsId)
	BEGIN
		UPDATE	SSNews_Products
		SET		
				NewsId = @NewsId,
				ProductId = @ProductId,
				CreatedDate = getdate()
		WHERE	ProductId = @ProductId  AND NewsId = @NewsId;
	END
	ELSE
	BEGIN
		INSERT INTO SSNews_Products(
		                NewsId, 
					    ProductId,
						CreatedDate 
				   )
				   VALUES(
				        @NewsId, 
					    @ProductId,
						GETDATE()
						)



							INSERT INTO ClubUsers
					(ClubId,
					UserId,
					ClubMemberId,
					CreatedDate)

			SELECT	
			data.col.value('(@ClubId)[1]', 'int') AS ClubId,
			data.col.value('(@UserDetailId)[1]', 'int') AS UserDetailId,
			'' AS ClubMemberId,
			GETDATE() AS CreatedDate
			FROM @XMLData.nodes('(/UserGroupData/UserRecord)') AS data(col);
		         
	END
		         
	END
	END


EXEC dbo.GetCouponRedemptions @NewsID=3595

exec SUPPORT_GET_ALL_COUPONS @ShowExpired=0,@CategoryID=5
EXEC SUPPORT_GET_COUPON_DETAILS @NewsId =2,@NewsCategoryID=4
EXEC SaveSSNewsUserAltNCRImpression

exec dbo.GetMFGCouponsForUpdate
exec dbo.GetMFGCouponsForNCR

EXEC GetAddedMFGCoupons
exec GetActiveCoupons
exec GetCouponsUsingSearchCode @ProductCategoryId=4,@UserId=0,@SearchCode='',@SearchValue=''



select * from ClientEnterprises
SELECT *
         FROM REDEMPTIONDISTRIBUTION


SELECT * FROM GetSSNewsWithRedeemCount 
EXEC GetSSNewsWithRedeemCount @SSNewsId=213,@CustomerID=1
select * from SSNewsUsersRedeem   ---- redeems

select * from SSNewsUsersNCRImpressions  where NCRImpressionCode= 'Offer is expired or shutoff. Unable to add to list'
SELECT * FROM SSNews
where NewsId = 213   ----clips
SELECT * FROM SSNews_NCRPromotions Where EnterpriseId='PH99CG8X6JTBW6VK38CSMGSVJW'
exec GetSSNewsWithRedeemCount_OLD @SSNewsId=3595,@CustomerID=1

select * from SSNews where NewsId =213
 -- 2018-08-03 13:21:35.933  UpdateDateTime
 --- 2018-08-03 13:21:37.977
--- 57XQ78VPD7WKYUZFDEUN2ZBBTA



select * from SSNewsUsersRedeem
select * from MyCart

select * from SSNews
exec  GetCouponsByCategoryAndDates
    @NewsCategoryID =5,      
    @ValidFromDate = '2018-08-29 19:00:00.000', 
    @ExpiresOn  = '2024-11-22 03:43:00.000'

	exec [dbo].[GetSSNewsWithRedeemCountAndClips] 
	 @NewsCategoryID =4,      
    @ValidFromDate = '2018-08-29', 
    @ExpiresOn  = '2024-11-22'

	select * from MFG_Coupons
---CREATE PROCEDURE [dbo].[GetSSNewsWithRedeemCountAndClips]  
ALTER PROCEDURE [dbo].[GetSSNewsWithRedeemCountAndClips]  
    @NewsCategoryID INT,
    @ValidFromDate DATETIME,
    @ExpiresOn DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Temporary table for redeem counts
    DECLARE @redeem TABLE (newsid INT, RedeemCount INT);

    INSERT INTO @redeem (newsid, RedeemCount)
    SELECT 
        ssnewsid, COUNT(*) AS RedeemCount
    FROM 
        SSNews_NCRPromotions 
    INNER JOIN
        RedemptionDistribution ON ncrpromotionid = promotionid
    GROUP BY 
        RedemptionDistribution.promotionid, ssnewsid;

    -- Main query
    SELECT 
        SN.NewsID,
        SN.NewsCategoryID,
        CASE 
            WHEN SN.NewsCategoryId = 4 THEN CONCAT(SN.Title, '-', SN.Details)
            ELSE SN.Title 
        END AS Title,
        SN.Details,
		SN.OtherDetails,
        COALESCE(P.ProductImage, 'CompanyLogo.png') AS ProductImage,
        SN.ImagePath,
        DATEADD(HOUR, 0, SN.ValidFromDate) AS ValidFromDate,
        DATEADD(HOUR, 0, SN.ExpiresOn) AS ExpiresOn,
        SN.SendNotification,
        SN.CustomerID,
        SN.CreateUserID,
        SN.UpdateUserID,
        C.CustomerName,
        NC.CategoryName,
        SN.PUICode,
        SN.CouponUniqueId,
        SN.ProductId,
        SN.Amount,
        CASE 
            WHEN SN.NewsCategoryId = 4 THEN 
                (SELECT BrandName FROM MFG_Coupons WHERE CouponOfferId = SN.ManufacturerCouponId)
            ELSE 
                P.ProductName 
        END AS ProductName,
        SN.CouponCode,
        SN.DiscountAmount,
        SN.IsDiscountPercentage,
        (SELECT TOP 1 NCRPromotionId FROM SSNews_NCRPromotions WHERE SSNewsId = SN.NewsId) AS NCRPromotionCode,
        SN.NCRPromotionCreatedDate,
        SN.IsItStoreSpecific,
        SN.ManufacturerCouponId,
        SN.ProductQuantity,
        SN.UpSellProductId,
        SN.UpSellProductQuantity,
        SN.IsFeatured,
        SN.IsItTargetSpecific,
        SN.IsRecurring,
        r.RedeemCount,
        SN.IsDealOftheWeek,
        SN.MFGShutOffDate,
        (SELECT COUNT(*) FROM SSNewsUsersNCRImpressions WHERE NewsId = SN.NewsId) AS ClipsCount,
        SN.ParentNewsId,
        SN.CouponLimit,
        C.IsPosIntegrationEnabled AS IsPosIntegrationEnabled,
        (SELECT TOP 1 OfferTypeId 
         FROM DISTRIBUTOR_COUPONS 
         WHERE Dist_Coupons_Offer_Id = SN.ManufacturerCouponId 
         ORDER BY 1 DESC) AS OfferTypeId
    FROM 
        SSNews AS SN
    INNER JOIN 
        NewsCategories AS NC ON NC.NewsCategoryID = SN.NewsCategoryID
    INNER JOIN 
        Customers AS C ON C.CustomerId = SN.CustomerId
    LEFT OUTER JOIN 
        Product AS P ON P.ProductId = SN.ProductId
    LEFT OUTER JOIN 
        @redeem r ON r.newsid = SN.NewsId

         WHERE SN.NewsCategoryID != 1 
        AND SN.NewsStatusID = 1  
        AND (@NewsCategoryID IS NULL OR SN.NewsCategoryID = @NewsCategoryID)
        AND (@ValidFromDate IS NULL OR SN.ValidFromDate >= @ValidFromDate)
        AND (@ExpiresOn IS NULL OR SN.ExpiresOn <= @ExpiresOn)
    ORDER BY 
        SN.CreateDatetime DESC;
END;




ALTER PROCEDURE GetCouponsByCategoryAndDates
    @NewsCategoryID INT = NULL,       -- Optional: Filter by NewsCategoryID
    @ValidFromDate DATETIME = NULL,  -- Optional: Filter by ValidFromDate
    @ExpiresOn DATETIME = NULL       -- Optional: Filter by ExpiresOn
AS
BEGIN
    SET NOCOUNT ON;

	

DECLARE @redeem TABLE (
    newsid INT,
    RedeemCount INT
);

-- Insert data into @redeem table variable
INSERT INTO @redeem (newsid, RedeemCount)
SELECT 
    ssnewsid, COUNT(*) AS total
FROM 
    SSNews_NCRPromotions
INNER JOIN
    RedemptionDistribution ON ncrpromotionid = promotionid
GROUP BY 
    RedemptionDistribution.promotionid, ssnewsid;


    SELECT DISTINCT  
        SN.NewsID,  
        SN.NewsCategoryID,  
        SN.Title,  
        CASE 
            WHEN SN.NewsCategoryID IN (3, 5, 6) THEN CONCAT(SN.Details, ' - ', SN.OtherDetails)  
            WHEN SN.NewsCategoryID = 4 THEN CONCAT(SN.Details, '||', 
                (SELECT TOP 1 OfferDisclaimer FROM MFG_COUPONS AS MF 
                 WHERE MF.CouponOfferId = SN.ManufacturerCouponId))  
            ELSE SN.Details  
        END AS Details,  
        P.ProductImage,  
        SN.ImagePath,  
        SN.ValidFromDate,  
        SN.ExpiresOn,  
        SN.SendNotification,  
        SN.CustomerID,  
        SN.CreateUserID,  
        SN.UpdateUserID,  
        C.CustomerName,  
        NC.CategoryName,  
        CASE 
            WHEN SN.NewsCategoryID = 8 THEN '100%' 
            ELSE 
                CASE 
                    WHEN SN.IsDiscountPercentage = 1 THEN CONCAT(SN.DiscountAmount, '%') 
                    ELSE 
                        CASE 
                            WHEN SN.DiscountAmount >= 1 THEN CONCAT('$', SN.DiscountAmount) 
                            ELSE REPLACE(CONCAT(SN.DiscountAmount, '¢'), '0.', '') 
                        END 
                END 
        END AS PUICode,   
        SN.CouponUniqueId,  
        SN.ProductId,  
        SN.Amount,  
        CASE 
            WHEN SN.NewsCategoryID = 6 THEN SN.Title  
            WHEN SN.NewsCategoryID = 4 THEN 
                (SELECT TOP 1 BrandName FROM MFG_COUPONS AS MF 
                 WHERE MF.CouponOfferId = SN.ManufacturerCouponId)  
            ELSE P.ProductName  
         END AS ProductName,  
        SN.CouponCode,  
        SN.DiscountAmount,  
        SN.IsDiscountPercentage,  
        SNP.NCRPromotionid AS NCRPromotionCode,  
        SN.NCRPromotionCreatedDate,  
        SN.IsFeatured,  
        PCG.ProductCategoryId,  
        (SELECT TOP 1 SSUR.RedeemOn 
         FROM SSNewsUsersRedeem AS SSUR
         WHERE SSUR.NewsId = SN.NewsId
         ORDER BY SSUR.RedeemOn DESC) AS RedeemOn,
       (SELECT COUNT(*) FROM SSNewsUsersNCRImpressions WHERE NewsId = SN.NewsId) AS ClipsCount,



        (SELECT TOP 1 SpecialPrice 
         FROM WeeklySpecialsExtension 
         WHERE NewsId = SN.NewsID) AS SpecialPrice,  
        '' AS DepartmentName,  
        SN.IsItTargetSpecific,
		 r.RedeemCount,
		
        ISNULL(NT.IsExclude, 2) AS IsExclude
    FROM SSNews AS SN  
    INNER JOIN NewsCategories AS NC ON NC.NewsCategoryID = SN.NewsCategoryID  
    INNER JOIN Customers AS C ON C.CustomerId = SN.CustomerId  
    LEFT OUTER JOIN SSNews_NCRPromotions AS SNP ON SNP.SSNewsId = SN.NewsID  
    INNER JOIN SelectedStoresForCoupons AS SSC ON SSC.NewsId = SN.NewsId  
	LEFT OUTER JOIN 
    @redeem r ON r.newsid = SN.newsid
	
    LEFT OUTER JOIN Product AS P ON P.ProductId = SN.ProductId  
    LEFT OUTER JOIN ProductCategories AS PCG ON PCG.ProductCategoryId = P.ProductCategoryId  
    LEFT OUTER JOIN Product_Major_Categories AS PMC ON PMC.ProductMajorCategoryID = PCG.MajorDepartmentID  
    LEFT OUTER JOIN NewsTargets AS NT ON NT.NewsId = SN.NewsId  
    WHERE SN.NewsCategoryID != 1 
        AND SN.NewsStatusID = 1  
        AND (@NewsCategoryID IS NULL OR SN.NewsCategoryID = @NewsCategoryID)
        AND (@ValidFromDate IS NULL OR SN.ValidFromDate >= @ValidFromDate)
        AND (@ExpiresOn IS NULL OR SN.ExpiresOn <= @ExpiresOn)
    ORDER BY SN.NewsID DESC;
END;


select * from RedemptionDistribution


select * from SSNewsUsersNCRImpressions  where NewsID=183           ---- NCRImpressionDate NewsId
 select * from SSNews  where NewsID=183  -----UpdateDateTime  NewsID


select * from SSNews_NCRPromotions 

 select * from CouponStatuses

 exec RSA_ETL_CouponRedemptions @StartDate='2018-05-17',@EndDate='2020-06-22'
exec RSA_ETL_CouponClips @StartDate='2018-05-17',@EndDate='2020-06-22'
exec RSA_ETL_BasketData @StartDate='2018-05-17',@EndDate='2020-06-22'
exec RSA_ETL_BasketCoupons @StartDate='2018-05-17',@EndDate='2020-06-22'
exec RSA_ETL_CouponProducts @StartDate='2018-05-17',@EndDate='2020-06-22'
exec Dist_GetDistributorCoupons @SSNewsId=3550
exec GetSSNews @SSNewsId =2,@CustomerID=1
exec dbo.GetSSNewsNCRPromotions @SSNewsId =2

	exec Get_CouponRedemptions @StartDate='2018-08-29',@EndDate='2018-08-29',@NewsCategoryID=4

	ALTER PROCEDURE [dbo].[Get_CouponRedemptions] 
    @StartDate AS DATE, 
    @EndDate AS DATE,
    @NewsCategoryID AS INT = NULL -- Optional parameter for filtering by NewsCategoryID
AS
BEGIN
    SELECT 
        SN.Newsid AS CouponID,
        SN.TITLE AS CouponTitle,
        SN.NEWSCATEGORYID AS NewsCategoryID, -- Added NewsCategoryID
        RD.MEMBERNUMBER AS MemberNumber,
        RD.ID AS RetailerRedemptionID,
        REPLACE(RD.REDEEMAMOUNT, 'USD', '') AS RedeemAmount,
        CONVERT(DATE, RD.RedemptionDate) AS RedemptionDate,
        CS.ClientStoreId AS RetailerStoreID,
        (SELECT TOP 1 CompanyID FROM Customers) AS RetailerID,
        (SELECT CASE DATEPART(WEEKDAY, CONVERT(DATE, RD.RedemptionDate))   
            WHEN 1 THEN 'SUNDAY'   
            WHEN 2 THEN 'MONDAY'   
            WHEN 3 THEN 'TUESDAY'   
            WHEN 4 THEN 'WEDNESDAY'   
            WHEN 5 THEN 'THURSDAY'   
            WHEN 6 THEN 'FRIDAY'   
            WHEN 7 THEN 'SATURDAY'   
        END) AS DayoftheWeek,
        GETDATE() AS DataExportTime,
        
        -- Count total redeems for each coupon
        (SELECT COUNT(*) 
         FROM REDEMPTIONDISTRIBUTION AS RD_Count
         WHERE RD_Count.PROMOTIONID = RD.PROMOTIONID
           AND CONVERT(DATE, RD_Count.RedemptionDate) BETWEEN @StartDate AND @EndDate
        ) AS TotalRedeems
    FROM REDEMPTIONDISTRIBUTION AS RD WITH (NOLOCK)
    INNER JOIN SSNEWS_NCRPROMOTIONS AS SSN WITH (NOLOCK) ON SSN.NCRPROMOTIONID = RD.PROMOTIONID
    INNER JOIN SSNEWS AS SN WITH (NOLOCK) ON SN.NEWSID = SSN.SSNEWSID
    INNER JOIN CLIENTSTORES AS CS ON CS.ClientStorePOSEnterpriseSecretId = RD.RedeemEnterpriseId
    WHERE SN.NEWSCATEGORYID <> 1 -- Existing condition
      AND CONVERT(DATE, RD.RedemptionDate) BETWEEN @StartDate AND @EndDate
      AND ( SN.NEWSCATEGORYID = @NewsCategoryID); -- Filter by NewsCategoryID if provided
END;

 exec SEARCH_COUPON_DETAILS  @NewsCategoryID=4 ,@Valid='2020-05-17 06:01:00.000',@Expires='2020-06-22 06:59:00.000'
   NewsID
   NewsCategoryID
   Title
   Details
   ProductImage
   ImagePath
   ValidFromDate
   ExpiresOn
   ShutOffDate
   CategoryName
   ProductId
   Amount
   ProductName
   IsItStoreSpecific
 ManufacturerCouponId
        ProductQuantity
      IsFeatured
       ProductCode,
        IsItTargetSpecific,
        OtherDetails,
       IsRecurring,
        RecurringId,
        DiscountAmount,
      IsDiscountPercentage,
       NCRPromotionID
	   RecurringEndDate,
        SN.IsDealOftheWeek,
        0 AS ISMultipleUPCCoupon


 CREATE PROCEDURE [dbo].[SEARCH_COUPON_DETAILS]
    @NewsCategoryID INT,
    @Valid DATETIME = NULL, -- Optional parameter
    @Expires DATETIME = NULL -- Optional parameter
AS
BEGIN
    -- Prevent extra result sets from interfering with the output
    SET NOCOUNT ON;

    -- Query to fetch the coupon details
    SELECT 
        SN.NewsID,
        SN.NewsCategoryID,
        SN.Title,
        SN.Details,
        P.ProductImage,
        SN.ImagePath,
        DATEADD(HOUR, 0, SN.ValidFromDate) AS ValidFromDate,
        DATEADD(HOUR, 0, SN.ExpiresOn) AS ExpiresOn,
        DATEADD(HOUR, 0, SN.MfgShutOffDate) AS ShutOffDate,
        NC.CategoryName,
        SN.ProductId,
        SN.Amount,
        P.ProductName,
        SN.IsItStoreSpecific,
        SN.ManufacturerCouponId,
        SN.ProductQuantity,
        SN.IsFeatured,
        P.ProductCode,
        SN.IsItTargetSpecific,
        SN.OtherDetails,
        SN.IsRecurring,
        SN.RecurringId,
        SN.DiscountAmount,
        SN.IsDiscountPercentage,
        SSN.NCRPromotionID,
        (SELECT TOP 1 RecurringEndDate 
         FROM SSNewsRecurrings 
         WHERE NewsId = SN.RecurringId) AS RecurringEndDate,
        SN.IsDealOftheWeek,
        0 AS ISMultipleUPCCoupon
    FROM SSNews AS SN
    INNER JOIN [dbo].[SSNews_NCRPromotions] AS SSN ON SSN.SSNewsID = SN.NewsID
    INNER JOIN NewsCategories AS NC ON NC.NewsCategoryID = SN.NewsCategoryID
    INNER JOIN Customers AS C ON C.CustomerId = SN.CustomerId
    LEFT JOIN SSNews_Products AS SP ON SP.NewsId = SN.NewsID
    INNER JOIN Product AS P ON P.ProductId = SP.ProductId
    WHERE 
        SN.NewsCategoryID = @NewsCategoryID
        AND ((@Valid IS NULL) OR (SN.ValidFromDate >= @Valid))
        AND ((@Expires IS NULL) OR (SN.ExpiresOn <= @Expires))
        AND SN.NewsStatusId = 1
    ORDER BY 
        SN.CreateDatetime DESC;
END;

---- 
find shoppers by	UPCS:
SELECT * FROM UserDetails
select * from SSNews_Departments

SELECT * FROM dbo.BasketItems;

select * from dbo.BasketConsumerIds

exec PROC_CUSTOM_GET_ALL_SHOPPER_GROUPS
select * from Clubs
select * from ClubUsers

select * from UserDetails

EXEC PROC_CUSTOM_UPLOAD_SHOPPERS_GROUPS_MEMBERNUMBER @GroupName='UPLOAD SHOPPER',@MemberNumbers='44170681221,44309517970'

---- UPLOAD SHOPPERS GROUPS:
CREATE PROC PROC_CUSTOM_UPLOAD_SHOPPERS_GROUPS_MEMBERNUMBER(
 @GroupName varchar(255),
 @MemberNumbers varchar(max)
)
 AS
	BEGIN
	  SET NOCOUNT ON;
	   BEGIN TRY
	    ---- Split members into individual values:
		 WITH MemberList AS(
			SELECT DISTINCT value AS MemberNumber
			FROM string_split(@MemberNumbers,',')
		 )
		  SELECT DISTINCT 
			    MemberNumber
            INTO #TempNumbers
			FROM MemberList

			DECLARE @MemberNumber VARCHAR(50),
			       @CustomGroupName VARCHAR(MAX),
				   @NewGroupID INT
			DECLARE db_TempMembers CURSOR FOR
			 SELECT MemberNumber FROM #TempNumbers
			 OPEN db_TempMembers
			 FETCH NEXT FROM db_TempMembers INTO @MemberNumber;
			 WHILE @@FETCH_STATUS =0
			 BEGIN
				SET  @CustomGroupName = CONCAT(@GroupName,'_',@MemberNumber);
				INSERT INTO Clubs(Name, ClubDetails, IsMemberIDRequired, IsEnableOnSignOn, CreatedDate, ModifiedDate)
				VALUES( @CustomGroupName,CONCAT('Group for ',@MemberNumber),0,0,GETDATE(),GETDATE() )

				SET @NewGroupID  = SCOPE_IDENTITY();
				INSERT INTO ClubUsers (ClubId,UserId,CreatedDate)
				 SELECT @NewGroupID, UD.UserDetailID,GETDATE()
				   FROM UserDetails UD
				   WHERE UD.BarCodeValue = @MemberNumber
				   AND IsDeleted = 0
				 FETCH NEXT FROM db_TempMembers INTO @MemberNumber;
			 END
			 
			 CLOSE db_TempMembers;
			 DEALLOCATE db_TempMembers;
			 DROP TABLE #TempNumbers;

	   END TRY
	     BEGIN CATCH
        -- Handle errors
        IF @@TRANCOUNT > 0
            ROLLBACK;

        -- Re-throw the error
        THROW;
    END CATCH
	END


update Clubs set Name = 'Veritra Special Group',ClubDetails='Veritra Special Group' where ClubId

EXEC PROC_CUSTOM_GET_SEARCH_COUNT  @MinDays=0,@MaxDays=100,@GroupName='Veritra Family  Group'


 EXEC PROC_CUSTOM_SEARCH_CREATE_GROUP  @MinDays=1,@MaxDays=100,@GroupName='Employees test group' 
---- Search and Create Grooup
ALTER PROC PROC_CUSTOM_SEARCH_CREATE_GROUP
(
    @MinDays INT,
    @MaxDays INT,
    @GroupName NVARCHAR(255)
)
AS
BEGIN
    BEGIN TRY
        -- Check if the group exists
        IF EXISTS (
            SELECT 1 
            FROM Clubs 
            WHERE Name = @GroupName
        )
        BEGIN
            RETURN;
        END
        
        -- Insert new group
        INSERT INTO Clubs (Name, IsMemberIDRequired, IsEnableOnSignOn, CreatedDate, ModifiedDate, ClubDetails)
        VALUES (@GroupName, 0, 0, GETDATE(), GETDATE(), @GroupName);

        -- Get the new ClubID
        DECLARE @NewClubID INT;
        SET @NewClubID = SCOPE_IDENTITY();

        -- Add users to the new group
        INSERT INTO ClubUsers (ClubId, UserId, CreatedDate)
        SELECT @NewClubID, UserDetailId, GETDATE()
        FROM UserDetails 
        WHERE IsDeleted = 0;
    END TRY
    BEGIN CATCH
        -- Handle errors
        THROW;
    END CATCH
END



ALTER PROC PROC_CUSTOM_SEARCH_CREATE_GROUP(
  @MinDays INT,
    @MaxDays INT,
    @GroupName NVARCHAR(255)
)
AS
	BEGIN
	 IF  EXISTS(
	  SELECT 1 FROM Clubs
	  WHERE Name =@GroupName
	 )
		begin
		 return ;
		end
		
		   INSERT INTO Clubs (Name, IsMemberIDRequired,IsEnableOnSignOn,CreatedDate,ModifiedDate,ClubDetails)
		   values(@GroupName,0,0,getdate(),getdate(),@GroupName)

		   DECLARE @NewClubID INT;
		   SET @NewClubID = SCOPE_IDENTITY();

		   INSERT INTO ClubUsers (ClubId,UserId,CreatedDate)
		  SELECT @NewClubID,UserDetailId,Getdate()
		  from UserDetails 
		  where IsDeleted = 0;
		    

		
		SELECT 
        Name AS GroupName, 
        ClubDetails AS GroupDetails, 
        ClubID AS GroupID,
        CAST(CreatedDate AS DATE) AS 'CreatedOn',
        (
            SELECT COUNT(DISTINCT UD.Barcodevalue)  
            FROM CLUBUSERS CU 
            INNER JOIN UserDetails UD ON UD.UserDetailID = CU.UserID
            WHERE CU.CLUBID = C2.ClubID
        ) AS TotalShoppers, -- Count of shoppers
        TopicARN
    FROM Clubs C2 
    WHERE 
        CAST(CreatedDate AS DATE) > DATEADD(DAY, -@MaxDays, GETDATE()) 
        AND CAST(CreatedDate AS DATE) <= DATEADD(DAY, -@MinDays, GETDATE())
        AND ClubID NOT IN (SELECT RewardGroupID FROM LM_REWARD_GROUPS)
        AND ClubID NOT IN (SELECT ClubID FROM CLUBS_DELETED)
        AND (Name = @GroupName ) 
    ORDER BY ClubID DESC;
	END


	ALTER PROC PROC_CUSTOM_SEARCH_CREATE_GROUP
(
    @MinDays INT,
    @MaxDays INT,
    @GroupName NVARCHAR(255)
)
AS
BEGIN
    BEGIN TRY
        -- Check if the group exists
        IF EXISTS (
            SELECT 1 
            FROM Clubs 
            WHERE Name = @GroupName
        )
        BEGIN
            RETURN;
        END
        
        -- Insert new group
        INSERT INTO Clubs (Name, IsMemberIDRequired, IsEnableOnSignOn, CreatedDate, ModifiedDate, ClubDetails)
        VALUES (@GroupName, 0, 0, GETDATE(), GETDATE(), @GroupName);

        -- Get the new ClubID
        DECLARE @NewClubID INT;
        SET @NewClubID = SCOPE_IDENTITY();

        -- Add users to the new group
        INSERT INTO ClubUsers (ClubId, UserId, CreatedDate)
        SELECT @NewClubID, UserDetailId, GETDATE()
        FROM UserDetails 
        WHERE IsDeleted = 0;

        -- Retrieve matching groups
        SELECT 
            Name AS GroupName, 
            ClubDetails AS GroupDetails, 
            ClubID AS GroupID,
            CAST(CreatedDate AS DATE) AS 'CreatedOn',
            (
                SELECT COUNT(DISTINCT UD.Barcodevalue)  
                FROM CLUBUSERS CU 
                INNER JOIN UserDetails UD ON UD.UserDetailID = CU.UserID
                WHERE CU.CLUBID = C2.ClubID
            ) AS TotalShoppers, -- Count of shoppers
            TopicARN
        FROM Clubs C2 
        WHERE 
            CAST(CreatedDate AS DATE) > DATEADD(DAY, -@MaxDays, GETDATE()) 
            AND CAST(CreatedDate AS DATE) <= DATEADD(DAY, -@MinDays, GETDATE())
            AND ClubID NOT IN (SELECT RewardGroupID FROM LM_REWARD_GROUPS)
            AND ClubID NOT IN (SELECT ClubID FROM CLUBS_DELETED)
            AND (Name = @GroupName)
        ORDER BY ClubID DESC;
    END TRY
    BEGIN CATCH
        -- Handle errors
        THROW;
    END CATCH
END



ALTER PROC PROC_CUSTOM_SEARCH_CREATE_GROUP(
  @MinDays INT,
    @MaxDays INT,
    @GroupName NVARCHAR(255)
)
AS
	BEGIN
	 IF NOT EXISTS(
	  SELECT 1 FROM Clubs
	  WHERE Name = @GroupName 
	 )
		BEGIN
		   INSERT INTO Clubs (Name, IsMemberIDRequired,IsEnableOnSignOn,CreatedDate,ModifiedDate,ClubDetails)
		   values(@GroupName,0,0,getdate(),getdate(),@GroupName)

		   DECLARE @NewClubID INT;
		   SET @NewClubID = SCOPE_IDENTITY();

		   INSERT INTO ClubUsers (ClubId,UserId,CreatedDate)
		  SELECT @NewClubID,UserDetailId,Getdate()
		  from UserDetails 
		  where IsDeleted = 0;
		    

		END
		SELECT 
        Name AS GroupName, 
        ClubDetails AS GroupDetails, 
        ClubID AS GroupID,
        CAST(CreatedDate AS DATE) AS 'CreatedOn',
        (
            SELECT COUNT(DISTINCT UD.Barcodevalue)  
            FROM CLUBUSERS CU 
            INNER JOIN UserDetails UD ON UD.UserDetailID = CU.UserID
            WHERE CU.CLUBID = C2.ClubID
        ) AS TotalShoppers, -- Count of shoppers
        TopicARN
    FROM Clubs C2 
    WHERE 
        CAST(CreatedDate AS DATE) > DATEADD(DAY, -@MaxDays, GETDATE()) 
        AND CAST(CreatedDate AS DATE) <= DATEADD(DAY, -@MinDays, GETDATE())
        AND ClubID NOT IN (SELECT RewardGroupID FROM LM_REWARD_GROUPS)
        AND ClubID NOT IN (SELECT ClubID FROM CLUBS_DELETED)
        AND (Name = @GroupName ) -- Filter by group name
    ORDER BY ClubID DESC;
	END
---- Search and count shoppers:

ALTER PROC PROC_CUSTOM_GET_SEARCH_COUNT(
    @MinDays INT,
    @MaxDays INT,
    @GroupName NVARCHAR(255))
AS
BEGIN
    SELECT 
        Name AS GroupName, 
        ClubDetails AS GroupDetails, 
        ClubID AS GroupID,
        CAST(CreatedDate AS DATE) AS 'CreatedOn',
        (
            SELECT COUNT(DISTINCT UD.Barcodevalue)  
            FROM CLUBUSERS CU 
            INNER JOIN UserDetails UD ON UD.UserDetailID = CU.UserID
            WHERE CU.CLUBID = C2.ClubID
        ) AS TotalShoppers, -- Count of shoppers
        TopicARN
    FROM Clubs C2 
    WHERE 
        CAST(CreatedDate AS DATE) > DATEADD(DAY, -@MaxDays, GETDATE()) 
        AND CAST(CreatedDate AS DATE) <= DATEADD(DAY, -@MinDays, GETDATE())
        AND ClubID NOT IN (SELECT RewardGroupID FROM LM_REWARD_GROUPS)
        AND ClubID NOT IN (SELECT ClubID FROM CLUBS_DELETED)
        AND (Name LIKE '%' + @GroupName + '%') -- Filter by group name
    ORDER BY ClubID DESC;
END;

--- edit
ALTER PROC PROC_CUSTOM_GET_SEARCH_COUNT(
    @MinDays INT,
    @MaxDays INT,
    @GroupName NVARCHAR(255))
AS
BEGIN
    SELECT 
        Name AS GroupName, 
        ClubDetails AS GroupDetails, 
        ClubID AS GroupID,
        CAST(CreatedDate AS DATE) AS 'CreatedOn',
        (
            SELECT COUNT(DISTINCT UD.Barcodevalue)  
            FROM CLUBUSERS CU 
            INNER JOIN UserDetails UD ON UD.UserDetailID = CU.UserID
            WHERE CU.CLUBID = C2.ClubID
        ) AS TotalShoppers, -- Count of shoppers
        TopicARN
    FROM Clubs C2 
    WHERE 
        CAST(CreatedDate AS DATE) > DATEADD(DAY, -@MaxDays, GETDATE()) 
        AND CAST(CreatedDate AS DATE) <= DATEADD(DAY, -@MinDays, GETDATE())
        AND ClubID NOT IN (SELECT RewardGroupID FROM LM_REWARD_GROUPS)
        AND ClubID NOT IN (SELECT ClubID FROM CLUBS_DELETED)
        AND (Name = @GroupName) -- Filter by group name
    ORDER BY ClubID DESC;
END;

CREATE PROC PROC_CUSTOM_GET_SEARCH_COUNT(
@MinDays INT,
@MaxDays INT,
@GroupName VARCHAR(255)
)
 AS
	BEGIN
	 SELECT Name as GroupName,
	 ClubDetails as GroupDetails,
	 ClubID as GroupID,
	 CAST (CreatedDate as date) as 'CreatedOn',
	 (
		SELECT COUNT(DISTINCT UD.Barcodevalue) from CLUBUSERS CU
		INNER JOIN UserDetails UD ON UD.UserDetailID  = CU.UserID
		WHERE CU.CLUBID = C2.ClubID
	 ) TotalShoppers,TopicARN
	 FROM Clubs C2
	 WHERE CAST(CreatedDate as date) > DATEADD(DAY, -@MaxDays,GETDATE())
	 AND CAST(CreatedDate as date) <= DATEADD(DAY, -@MaxDays,GETDATE())
	 AND ClubID NOT IN (SELECT RewardGroupID FROM LM_REWARD_GROUPS)
	 AND ClubID NOT IN (SELECT ClubID FROM CLUBS_DELETED)
	 AND (Name LIKE '%' + @GroupName + '%' )
	 ORDER BY ClubID DESC
		
	END

select * from dbo.BasketCoupons
select * from dbo.BasketCouponUPC
SELECT * FROM dbo.BasketData
select * from clubs
exec [dbo].[PROC_CUSTOM_GET_ALL_SHOPPER_GROUPS]@GroupID =0, @UserID =0
select * from [dbo].[GetCommaSeparatedStringAsTable]

EXEC PROC_CUSTOM_GET_USER_RECENT_PURCHASED_PRODUCTS @UserId=0,@BasketDataId=0

EXEC PROC_CUSTOM_GET_SHOPPERS_PURCHASED_PRODUCTS @UPC='15',@NoOfCoupons=0,@FromDate='2018-08-29',@ToDate='2019-08-29'







exec [dbo].[PROC_CUSTOM_FIND_PRODUCTS] @ProductCode='',@ProductName='',@ProductCategoryId=0,@IsMajorDepartment='true'
select * from ProductCategories where ProductCategoryId = 1

SELECT * FROM dbo.Product

SELECT * FROM Product

select * from BasketData

drop proc CUSTOM_GET_PRODUCT_DETAILS
CREATE PROCEDURE CUSTOM_GET_PRODUCT_DETAILS
(
    @ProductName VARCHAR(50),
    @ProductCode VARCHAR(50),
    @ProductCategoryId INT
)
AS
BEGIN
    -- Check if all parameters indicate "fetch all data"
    IF @ProductCategoryId = 0 AND (@ProductName IS NULL OR @ProductName = '') AND (@ProductCode IS NULL OR @ProductCode = '')
    BEGIN
        -- Return all products with their category names
        SELECT 
            p.ProductCode,
            p.ProductName,
            pc.ProductCategoryName
        FROM 
            Product p
        INNER JOIN 
            ProductCategories pc 
        ON 
            p.ProductCategoryId = pc.ProductCategoryId;
    END
    ELSE
    BEGIN
        -- Declare a variable to store the ProductCategoryName
        DECLARE @ProductCategoryName VARCHAR(50);

        -- Fetch the ProductCategoryName only if ProductCategoryId is specified
        IF @ProductCategoryId > 0
        BEGIN
            SELECT @ProductCategoryName = pc.ProductCategoryName
            FROM ProductCategories pc
            WHERE pc.ProductCategoryId = @ProductCategoryId;
        END

        -- Fetch filtered product details
        SELECT 
            p.ProductCode,
            p.ProductName,
            @ProductCategoryName AS ProductCategoryName
        FROM 
            Product p
        WHERE 
            (@ProductCategoryId = 0 OR p.ProductCategoryId = @ProductCategoryId) AND
            (@ProductName IS NULL OR @ProductName = '' OR p.ProductName = @ProductName) AND
            (@ProductCode IS NULL OR @ProductCode = '' OR p.ProductCode = @ProductCode);
    END
END;
GO

EXEC CUSTOM_GET_PRODUCT_DETAILS @ProductName='',@ProductCode='',@ProductCategoryId=0

CREATE PROC CUSTOM_GET_PRODUCT_DETAILS (
 @ProductName VARCHAR(50),
 @ProductCode VARCHAR(50),
 @ProductCategoryId INT
)
AS
	BEGIN
	   IF @ProductCategoryId=0 AND (@ProductName IS NULL OR @ProductName='') AND (@ProductCode IS NULL OR @ProductCode='')
	   BEGIN
			SELECT
				  p.ProductCode,
				  p.ProductName,
				  pc.ProductCategoryName
			 FROM Product p
			  INNER JOIN
				     ProductCategories pc 
				  ON p.ProductCategoryId = pc.ProductCategoryId ;
	   END
	   ELSE
		BEGIN
		   DECLARE	@ProductCategoryName  VARCHAR(50);
		     IF @ProductCategoryId > 0
			  BEGIN
			    SELECT @ProductCategoryName = pc.ProductCategoryName
				FROM ProductCategories pc 
				WHERE pc.ProductCategoryId= @ProductCategoryId;
			  END

			  SELECT p.ProductCode,
			         p.ProductName,
					 ISNULL( @ProductCategoryName,pc.ProductCategoryName) AS ProductCategoryName
					 FROM Product p
					   INNER JOIN
						ProductCategories pc 
						ON p.ProductCategoryId = pc.ProductCategoryId

						WHERE
						  (@ProductCategoryId = 0 OR p.ProductCategoryId = @ProductCategoryId) AND
						  (
						  @ProductName IS NULL OR @ProductName = '' OR p.ProductName LIKE '%' + @ProductName + '%' ) AND
						  (@ProductCode IS NULL OR @ProductCode = '' OR p.ProductCode LIKE '%' + @ProductCode + '%')
						  

		END
	END

CUSTOM_GET_PRODUCT_DETAILS @ProductName='Basket',@ProductCode='2744',@ProductCategoryId=1



----- advance search shoppers
 exec [dbo].[PROC_CUSTOM_GET_SHOPPERS_ADVANCED_SEARCH]
 @MemberNumber='0'
 
 ,@TransactionFrom='2024-12-05',
 @TransactionTo='2024-12-12',
 @MinSpend=0,
 @MaxSpend=100,
 @Clubid='0',
 @MinBasketCount=0,
 @MaxBasketCount=0,
 @StoreId='0',
 @MinRedeemCount=0,
 @MaxRedeemCount=1,
 @DepartmentId=0,
 @IsCreateGroup=false




 exec [dbo].[PROC_CUSTOM_GET_SHOPPERS_ADVANCED_SEARCH]
    @MemberNumber='0',
    @TransactionFrom='2024-12-05',
    @TransactionTo='2024-12-12',
    @MinSpend=0,
    @MaxSpend=0,
    @Clubid='0',
    @MinBasketCount=0,
    @MaxBasketCount=0,
    @StoreId=0,
    @MinRedeemCount=0,
    @MaxRedeemCount=0,
    @DepartmentId=0,
    @IsCreateGroup=0;

--- search products
select * from Product

select * from ProductCategories

ALTER PROC CUSTOM_PROC_GET_PRODUCT_CATEGORIES
AS
	BEGIN
	 SELECT ProductCategoryId,ProductCategoryName,ClientDepartmentID,DefaultProductID,MajorDepartmentID  FROM ProductCategories
	END
exec CUSTOM_PROC_GET_PRODUCT_CATEGORIES
select * from Product_Major_Categories
select * from BasketItems
select * from BasketData

select * from UserDetails

update UserDetails set CustomerID = 1 WHERE UserDetailId=3489


SELECT * FROM CLUBS;
SELECT * FROM CLUBUSERS WHERE ClubId=99; 


EXEC dbo.PROC_CUSTOM_GET_TOP_CUSTOMERS
@NOOFRECORDS =50,
@STOREID=1,
@ORDERBYDIRECTION='ASC',
@BEGINDATE='2019-06-23',
@ENDDATE='2024-12-12',
@DEPARTMENTID=0

EXEC  PROC_CUSTOM_GET_MAJOR_DEPARTMENTS
EXEC PROC_CUSTOM_GET_MINOR_DEPARTMENTS


EXEC PROC_CUSTOM_SEARCH_TOP_PRODUCTS @NumberOfRecords=50,@ProductCategoryID=7,@StoreId=0,@TransactionFromDate='2020-12-11 20:02:19.303',@TransactionEndDate='2021-01-05 17:10:26.217',@IsMajorDepartment='true'

----pre-defined groups
select * from UserDetails where UserDetailId=3461
exec PROC_CREATE_UPC_SHOPPER @UPC='8473,87473,847',@NumberofTimesBought=1,@DuringLastNoOfDays=7,@GroupName='UPC DEMO'

alter PROCEDURE [dbo].[PROC_CREATE_UPC_SHOPPER]
    @UPC VARCHAR(MAX),
    @NumberofTimesBought INT,
    @DuringLastNoOfDays INT,
    @GroupName VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Variables
    DECLARE @minBasketCount INT = 0;
    DECLARE @maxBasketCount INT = 0;
    DECLARE @FromDate DATETIME;
    DECLARE @ToDate DATETIME = GETDATE();
    DECLARE @NewGroupID INT;

    -- Dynamically determine basket counts based on @NumberofTimesBought
    IF @NumberofTimesBought = 1
    BEGIN
        SET @minBasketCount = 1;
        SET @maxBasketCount = 3;
    END
    ELSE IF @NumberofTimesBought = 2
    BEGIN
        SET @minBasketCount = 4;
        SET @maxBasketCount = 6;
    END

    -- Calculate FromDate based on @DuringLastNoOfDays
    SET @FromDate = DATEADD(DAY, -@DuringLastNoOfDays, @ToDate);

    -- Append timestamp to @GroupName for uniqueness
    --SET @GroupName = CONCAT(@GroupName, '-', FORMAT(GETDATE(), 'yyyyMMddHHmmss'));
	SET @GroupName = Concat(@GroupName,'-',(SELECT CONVERT(VARCHAR(8), GETDATE(), 112) + REPLACE(CONVERT(varchar, GETDATE(), 108), ':','')))

    -- Insert into CLUBS table
    INSERT INTO CLUBS (Name, clubdetails, IsMemberIDRequired, IsEnableOnSignOn, createddate, ModifiedDate)
    VALUES (
        @GroupName,
        CONCAT('Group for UPC: ', SUBSTRING(@UPC, 1, 800), 
               ', Times Bought: ', @minBasketCount, ' to ', @maxBasketCount, 
               ', During Last ', @DuringLastNoOfDays, ' days.'),
        0, -- IsMemberIDRequired
        0, -- IsEnableOnSignOn
        GETDATE(), -- CreatedDate
        GETDATE()  -- ModifiedDate
    );

    -- Get the newly created Club ID
    SET @NewGroupID = SCOPE_IDENTITY();

    -- Insert shoppers into CLUBUSERS based on criteria
    ;WITH ShoppersCTE AS (
        SELECT DISTINCT 
            BCI.LoyaltyId
        FROM 
            BasketItems AS BI
            INNER JOIN BasketData AS BD ON BI.BasketDataId = BD.BasketDataId
            INNER JOIN BasketConsumerIds AS BCI ON BD.BasketDataId = BCI.BasketDataId
        WHERE 
            BI.UPC IN (SELECT COLUMN1 FROM [dbo].[GetCommaSeparatedStringAsTable](@UPC))
            AND BD.TransactionDate BETWEEN @FromDate AND @ToDate
        GROUP BY 
            BCI.LoyaltyId
        HAVING 
            COUNT(DISTINCT BI.BasketDataID) BETWEEN @minBasketCount AND @maxBasketCount
    )
    INSERT INTO CLUBUSERS (ClubID, UserID, CreatedDate)
    SELECT 
        @NewGroupID, 
        UD.UserDetailID, 
        GETDATE()
    FROM 
        ShoppersCTE AS CTE
        INNER JOIN UserDetails AS UD ON CTE.LoyaltyId = UD.BarcodeValue
    WHERE 
        UD.IsDeleted = 0;

  --  PRINT 'Club and associated users created successfully.';
END;


EXEC PROC_CUSTOM_CREATE_SHOPPER_GROUP_BY_LAST_SHOPPED_DATE @NoOfDaysSinceShopped=2,@GroupName=''
DECLARE @registered DATETIME = GETDATE(); -- Declare and initialize the variable
EXEC PROC_CUSTOM_CREATE_SHOPPER_GROUP_BY_ZIPCODE 
    @ZipcodeList = '507164,329521', 
    @AllUsers = 0, 
    @SinceRegistered = NULL;

EXEC	PROC_CUSTOM_GET_SHOPPER_PRODUCT_TIMES_BOUGHT
@UPC='',
@NumberofTimesBoughT=0,
@DuringLastNoOfDays=7,
@GroupName='',
@FromDate='',
@ToDate='',
@StoreID=0

EXEC PROC_CUSTOM_CREATE_SHOPPER_GROUP_BY_UPC_LIST
@GroupName = 'SAI TEST',
@UPCList='114226,28223,38345',
@NoOfTimesPurchased=2,
@NoOfDaysSinceShopped=7


---- download shoppers
 
EXEC PROC_CUSTOM_DOWNLOAD_SHOPPERS @GroupId =179
ALTER PROC PROC_CUSTOM_DOWNLOAD_SHOPPERS(
 @GroupId INT
)
   AS
	BEGIN
	   
	   SET NOCOUNT ON;

		SELECT UD.UserName,
				UD.BarcodeValue,
				CS.StoreName
	    FROM UserDetails UD
		INNER JOIN 
			ClubUsers CU ON UD.UserDetailId = CU.UserId
	    INNER JOIN
		    ClientStores CS ON UD.ClientStoreId  = CS.ClientStoreId
		WHERE
			CU.ClubId = @GroupId
		AND UD.IsDeleted = 0
		
	END
--- created by upcs list:
DECLARE @SinceToday DATETIME = GETDATE();

EXEC PROC_CUSTOM_CREATE_SHOPPER_GROUP_BY_UPC_LIST
@GroupName = 'SAI TEST',
@UPCList='114226,28223,38345',
@NoOfTimesPurchased=2,
@NoOfDaysSinceShopped=7





ALTER PROCEDURE [dbo].[PROC_CUSTOM_CREATE_SHOPPER_GROUP_BY_UPC_LIST]
    @GroupName VARCHAR(MAX), -- Base name for custom groups
    @UPCList VARCHAR(MAX),   -- Comma-separated UPC list
    @NoOfTimesPurchased INT, -- Minimum purchase count
    @NoOfDaysSinceShopped INT -- Days since last shopped
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Calculate FromDate and ToDate
        DECLARE @FromDate DATE, @ToDate DATE;
        SET @FromDate = DATEADD(DAY, -@NoOfDaysSinceShopped, GETDATE());
        SET @ToDate = GETDATE();

        -- Split the UPC list into individual values
        ;WITH UPCList AS (
            SELECT DISTINCT value AS UPC
            FROM STRING_SPLIT(@UPCList, ',')
        )
        SELECT DISTINCT
            UPC
        INTO #TempUPCs
        FROM UPCList;

        -- Process each UPC
        DECLARE @UPC VARCHAR(50), 
                @CustomGroupName VARCHAR(MAX), 
                @NewGroupID INT;

        -- Cursor to loop through UPCs
        DECLARE db_TempUPCs CURSOR FOR 
        SELECT UPC FROM #TempUPCs;

        OPEN db_TempUPCs;

        FETCH NEXT FROM db_TempUPCs INTO @UPC;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Generate a unique group name for the current UPC
            SET @CustomGroupName = CONCAT(@GroupName, '_', @UPC);

            -- Insert into CLUBS table
            INSERT INTO CLUBS (Name, ClubDetails, IsMemberIDRequired, IsEnableOnSignOn, CreatedDate, ModifiedDate)
            VALUES (@CustomGroupName, CONCAT('Group for UPC ', @UPC), 0, 0, GETDATE(), GETDATE());

            -- Retrieve the newly inserted ClubID
            SET @NewGroupID = SCOPE_IDENTITY();

            -- Insert matching users into CLUBUSERS, excluding those meeting the NOT IN condition
            INSERT INTO CLUBUSERS (ClubID, UserID, CreatedDate)
            SELECT @NewGroupID, UD.UserDetailID, GETDATE()
            FROM UserDetails UD
            INNER JOIN BasketConsumerIds BCI ON UD.BarcodeValue = BCI.LoyaltyID
            INNER JOIN BasketData BD ON BD.BasketDataId = BCI.BasketDataID
            WHERE UD.IsDeleted = 0
              AND BCI.LoyaltyID = @UPC
              AND BD.TransactionDate >= @FromDate
              AND (SELECT COUNT(*)
                   FROM BasketConsumerIds BCI2
                   WHERE BCI2.LoyaltyID = UD.BarcodeValue
                     AND BCI2.BasketDataID IN (
                         SELECT BasketDataID
                         FROM BasketData
                         WHERE TransactionDate >= @FromDate
                     )) >= @NoOfTimesPurchased
              AND UD.UserDetailID NOT IN (
                  SELECT DISTINCT UD2.UserDetailID
                  FROM BasketConsumerIds (NOLOCK) AS BCI2
                  INNER JOIN BasketData (NOLOCK) AS BD2 ON BD2.BasketDataId = BCI2.BasketDataID
                  INNER JOIN UserDetails (NOLOCK) AS UD2 ON UD2.BarcodeValue = BCI2.LoyaltyID AND UD2.IsDeleted = 0
                  WHERE BD2.TransactionDate BETWEEN @FromDate AND @ToDate
              );

            FETCH NEXT FROM db_TempUPCs INTO @UPC;
        END;

        CLOSE db_TempUPCs;
        DEALLOCATE db_TempUPCs;

        -- Drop temp table
        DROP TABLE #TempUPCs;

    END TRY
    BEGIN CATCH
        -- Handle errors
        PRINT 'Error Occurred: ' + ERROR_MESSAGE();
        THROW;
    END CATCH;
END;

select * from Clubs
SELECT * FROM ClubUsers where ClubId= 176   

SELECT * FROM ClubUsers Where ClubId = 99
select * from UserDetails Where USERDETAILID = 3470
EXEC PROC_CUSTOM_DOWNLOAD_SHOPPERS @GroupId = 179

EXEC GetClientStores @ClientStoreId =4
exec GetClientStoreUsers @ClientStoreId =4

exec PROC_CUSTOM_GET_CLIENT_STOREUSERS @ClientStoreId=4

	
	
SELECT * FROM UserDetails

EXEC PROC_CUSTOM_GET_GROUP_ANALYSIS_TIMELINE @GroupID=99,@UserID=0
EXEC PROC_CUSTOM_GET_ALL_SHOPPER_GROUPS @GroupID=0,@UserID=0
EXEC PROC_CUSTOM_GET_GROUP_TOP_PRODUTS @GROUPID=0,@NOOFDAYS=7
EXEC PROC_CUSTOM_GET_ALLTIME_PRODUTS @GrouPId=167


exec GetPagedSortedFilteredUserDetails @PageNumber=1,@PageSize=2,@SortColumn='Email',@SortDirection='asc',@SearchTerm=''
UPDATE  CLUBS SET Name='RSA Demo 7',ClubDetails ='RSA Demo 7' where ClubId=121
Select * from UserDetails where UserName = 'rhuicochea0@gmail.com'
select * from UserTypes
select * from UserStatuses
select * from webpages_Roles
select * from dbo.aspnet_Roles
select * from GetUsersRoles
select * from dbo.aspnet_Users where UserName = 'rhuicochea0@gmail.com'
select * from dbo.aspnet_membership where Email = 'rhuicochea0@gmail.com'

exec dbo.GetUserTypes
exec dbo.GetUsersRoles @CurrentUserId=1
EXEC GetRoles
---- Create Shopper proc
exec ValidateUserName @UserName='pusam@gmail.com'

CREATE proc [dbo].[SaveShopperDetailsData](
--  @UserDetailId int,
  @UserName varchar(150),
  @Email varchar(150),
  @Mobile varchar(15),
 @UserGUID uniqueidentifier,
 @CustomerCode varchar(150),

  @IsActive bit,
   @CreatedDate datetime,
 @ModifiedDate datetime,
 @FirstName varchar(50),
 @LastName varchar(15),
 @DeviceId int,
 @DeviceType varchar(50),
 @UserStatusId int,
 @IsDeleted bit,
 @BarCodeImage varchar(50),
 @BarCodeValue varchar(50),
 @UserTypeId int,
 @ZipCode varchar(10),
 @ClientStoreId int,
 @QToken varchar(50)

)
as
  begin 
	 INSERT INTO dbo.UserDetails(UserName,Email,Mobile,UserID,xCustomerCode,IsActive,CreatedDate,ModifiedDate,FirstName,LastName,DeviceId,DeviceType,UserStatusId,IsDeleted,BarCodeImage,BarCodeValue,UserTypeId,ZipCode,ClientStoreId,QToken)
	 VALUES(@UserName,@Email,@Mobile, @UserGUID,@CustomerCode,@IsActive,@CreatedDate,@ModifiedDate,@FirstName,@LastName,@DeviceId,@DeviceType,@UserStatusId,@IsDeleted,@BarCodeImage,@BarCodeValue,@UserTypeId,@ZipCode,@ClientStoreId,@QToken)
  end
Create PROCEDURE [dbo].[SaveShopperDetails]   
(  
 @UserDetailId int,   
 @UserName  varchar(150),  
 @Email   varchar(150),  
 @Mobile   varchar(15),  
 @CustomerId  uniqueidentifier,  
 @CustomerCode varchar(50),  
 @FirstName      varchar(50),  
 @LastName  varchar(15),  
 @DeviceId  int,  
 @DeviceType     varchar(50),  
 @UserStatusId  int,  
 @IsDeleted bit,  
 @BarCodeImage varchar(50),  
 @BarCodeValue varchar(50),  
 @QRCodeImage varchar(50),  
 @QRCodeValue varchar(50),  
 @CompanyCustomerFK int,  
 @UserTypeId int,  
 @ZipCode varchar(10),  
 @ClientStoreId int,  
 @QToken varchar(100)  
  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
  
 MERGE INTO UserDetails UD  
  USING (SELECT @UserDetailId as UserDetailId) S ON UD.UserDetailId = S.UserDetailId  
  WHEN MATCHED THEN  
   UPDATE  
    SET Email = @Email,  
     Mobile = @Mobile,  
     ModifiedDate=GETDATE(),  
     FirstName = @FirstName,  
     LastName = @LastName,  
     UserTypeId=@UserTypeId,  
     CustomerID=1,  
     ZipCode = @ZipCode,  
     UserName = @UserName  
   WHEN NOT MATCHED THEN  
   INSERT   
   (  
    UserName,  
    Email,  
    Mobile,  
    UserID,  
    xUserId,  
    xCustomerCode,  
    IsActive,  
    CreatedDate,  
    ModifiedDate,  
    FirstName,  
    LastName,  
    DeviceId,  
    DeviceType,  
    UserStatusId,  
    IsDeleted,  
    BarCodeImage,  
    BarCodeValue,  
    QRCodeImage,  
    QRCodeValue,  
    CustomerID,  
    UserTypeId,  
    ZipCode,  
    ClientStoreId,  
    QToken,  
    SignUpDate  
   )  
   VALUES  
   (  
    @UserName,  
    @Email,  
    @Mobile,  
    @CustomerId,  
    0,  
    @CustomerCode,  
    1,  
    GETDATE(),  
    GETDATE(),  
    @FirstName,  
    @LastName,  
    @DeviceId,  
    @DeviceType,  
    @UserStatusId,  
    @IsDeleted,  
    @BarCodeImage,  
    @BarCodeValue,  
    @QRCodeImage,  
    @QRCodeValue,  
    @CompanyCustomerFK ,  
    @UserTypeId,  
    @ZipCode,  
    @ClientStoreId,  
    @QToken,  
    DATEADD( HH, -5,GETDATE())  
   );  
  
   SET @UserDetailId = scope_identity()  
  
  EXEC ProcessDefaultCouponsForUser @UserDetailId  
  
END  
  
---- validate user
CREATE PROC ValidateUserName(
@UserName varchar(50)
)
 AS
	BEGIN
	 SELECT COUNT(1) FROM dbo.aspnet_Membership Where Email = @UserName
	END
	EXEC Get_Membership_user  @UserName='praveen@gmail.com'
	
	---- ValidateRole
	alter proc ValidateExistingRole(
	@RoleName VARCHAR(100)
	)
	 AS 
		BEGIN
		  select COUNT(1) from dbo.aspnet_Roles WHERE RoleName = @RoleName
		END
	EXEC ValidateExistingRole @RoleName='StoreAdmin'
	
	exec [dbo].[aspnet_Roles_RoleExists] @ApplicationName='/',@RoleName='SupperAdmin'
exec ValidateUserName @UserName='praveen@gmail.com';
exec dbo.Get_Membership_user @UserName='praveen@gmail.com'
select UserName from dbo.aspnet_Users where UserName='sai@gmail.com'
----- Create shopper 
Select * from UserDetails
select * from UserTypes
select * from UserStatuses
select * from webpages_Roles
select * from dbo.aspnet_Roles
select * from GetUsersRoles
select * from dbo.aspnet_Users

select * from aspnet_Membership where Email = 'sai@gmail.com'
select * from AspNetUsers
select * from dbo.aspnet_Membership

DECLARE @CurrentTimeUtc DATETIME = GETUTCDATE();
EXEC aspnet_UsersInRoles_AddUsersToRoles @ApplicationName='/',@UserNames='sai@gmail.com',@RoleNames='PromotionManager',@CurrentTimeUtc=@CurrentTimeUtc
exec aspnet_Roles_CreateRole @ApplicationName='/',@RoleName='SupperAdmin'

EXEC GetAspnetUserID @UserName='sai@gmail.com'


---- save user details 


---- create proc create shoppers
create proc CreateShopperDetails(

  @UserName varchar(150),
  @Email varchar(150),
  @Mobile varchar(15),
 @UserGUID uniqueidentifier,
 @CustomerCode varchar(150),

  @IsActive bit,
   @CreatedDate datetime,
 @ModifiedDate datetime,
 @FirstName varchar(50),
 @LastName varchar(15),
 @DeviceId int,
 @DeviceType varchar(50),
 @UserStatusId int,
 @IsDeleted bit,
 @BarCodeImage varchar(50),
 @BarCodeValue varchar(50),
 @UserTypeId int,
 @ZipCode varchar(10),
 @ClientStoreId int,
 @QToken varchar(50)

)
as
  begin 
	 INSERT INTO dbo.UserDetails(UserName,Email,Mobile,UserID,xCustomerCode,IsActive,CreatedDate,ModifiedDate,FirstName,LastName,DeviceId,DeviceType,UserStatusId,IsDeleted,BarCodeImage,BarCodeValue,UserTypeId,ZipCode,ClientStoreId,QToken)
	 VALUES(@UserName,@Email,@Mobile, @UserGUID,@CustomerCode,@IsActive,@CreatedDate,@ModifiedDate,@FirstName,@LastName,@DeviceId,@DeviceType,@UserStatusId,@IsDeleted,@BarCodeImage,@BarCodeValue,@UserTypeId,@ZipCode,@ClientStoreId,@QToken)
  end
EXEC SaveUserDetailsData @UserName,@Email,@Mobile,@UserGUID,@CustomerCode,@IsActive,@CreatedDate,@ModifiedDate,@FirstName,@LastName,@DeviceId,@DeviceType,@UserStatusId,@IsDeleted,@BarCodeImage,@BarCodeValue,@UserTypeId,@ZipCode,@ClientStoreId,@QToken
DECLARE @CurrentTimeUtc DATETIME = GETUTCDATE();
DECLARE @CreateDate DATETIME = GETDATE();
DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
EXEC dbo.aspnet_Membership_CreateUser 
    @ApplicationName = '/',
    @UserName = 'praveen@gmail.com',
    @Password = 'praveen123',
    @PasswordSalt = 'praveen123',
    @Email = 'praveen@gmail.com',
    @PasswordQuestion = 1,
    @PasswordAnswer = 1,
    @IsApproved = 1,
    @CurrentTimeUtc =  @CurrentTimeUtc,
    @CreateDate =  @CreateDate,     
    @UniqueEmail = 1,
    @PasswordFormat = 1,
    @UserId = @UserId;


--- get roles  
exec dbo.aspnet_Roles_GetAllRoles @ApplicationName='/'
--- create group shopper
select * FROM Clubs where ClubId=80
SELECT * FROM ClubUsers where UserId =3481 AND ClubId =157
select * from UserDetails Where UserDetailId=3461
--- update
update Clubs set  ClubDetails='$5 off your NEXT visit1' where ClubId =80
---- get user groups 
EXEC GetUserGroupsByUserId @UserId=3461; ----- PRESENT GROUPS
EXEC GetAvailableGroups @UserId=3469;    ---- ADDITIONAL GROUPS
EXEC AddAvailableGroups @UserId = 3481,@ClubId=103 --- Add to clubusers

                 --------------    EXEC DeleteGroupUserByUserIdandClubId @UserId = 3481,@ClubId=5

---- DeleteUserGROUP
ALTER PROC DeleteGroupUserByUserIdandClubId(
 @UserId INT,
 @ClubId INT
)
AS
	BEGIN
	
				DELETE FROM ClubUsers WHERE ClubId = @ClubId and UserId = @UserId;

				DELETE FROM Clubs             
				  WHERE ClubId = @ClubId
				
				  AND NOT EXISTS(
					SELECT 1
					FROM ClubUsers
					 Where ClubId = @ClubId
					)
				  
				  
	END

sai group name-
SAI TEST
create proc GetUserGroupsByUserId(
@UserId INT
)
 AS
	BEGIN
		SELECT c.ClubId, c.Name, c.clubdetails, c.IsMemberIDRequired, c.IsEnableOnSignOn, c.createddate, c.ModifiedDate 
 
		From ClubUsers cu
		JOIN clubs c on cu.ClubId = c.ClubId
		Where UserId = @UserId
	END


	==== ----------------\\
	CREATE OR ALTER PROCEDURE GetUserGroupsByUserId
(
    @UserId INT
)
AS
BEGIN
    -- Use ROW_NUMBER() to ensure unique group names
    WITH UniqueClubs AS (
        SELECT 
            c.ClubId,
            c.Name,
            c.clubdetails,
            c.IsMemberIDRequired,
            c.IsEnableOnSignOn,
            c.createddate,
            c.ModifiedDate,
            ROW_NUMBER() OVER (PARTITION BY c.Name ORDER BY c.CreatedDate DESC) AS RowNum
        FROM ClubUsers cu
        JOIN Clubs c ON cu.ClubId = c.ClubId
        WHERE cu.UserId = @UserId
    )
    -- Select only the distinct group names
    SELECT DISTINCT 
        Name  -- Only select Name to ensure uniqueness
    FROM UniqueClubs
    WHERE RowNum = 1;
END;
=======------------------ \\\\   

	CREATE OR ALTER PROCEDURE GetUserGroupsByUserId
(
    @UserId INT
)
AS
BEGIN
    -- Use ROW_NUMBER() to ensure unique group names
    WITH UniqueClubs AS (
        SELECT 
            c.ClubId,
            c.Name,
            c.clubdetails,
            c.IsMemberIDRequired,
            c.IsEnableOnSignOn,
            c.createddate,
            c.ModifiedDate,
            ROW_NUMBER() OVER (PARTITION BY c.Name ORDER BY c.CreatedDate DESC) AS RowNum
        FROM ClubUsers cu
        JOIN Clubs c ON cu.ClubId = c.ClubId
        WHERE cu.UserId = @UserId
    )
    -- Select only the first row (RowNum = 1) for each unique group name
    SELECT 
        Name  -- Only select Name to ensure uniqueness
    FROM UniqueClubs
    WHERE RowNum = 1;
END;


	CREATE OR ALTER PROCEDURE GetUserGroupsByUserId
(
    @UserId INT
)
AS
BEGIN
    -- Use ROW_NUMBER() to ensure unique club names
    WITH UniqueClubs AS (
        SELECT 
            c.ClubId,
            c.Name,
            c.clubdetails,
            c.IsMemberIDRequired,
            c.IsEnableOnSignOn,
            c.createddate,
            c.ModifiedDate,
            ROW_NUMBER() OVER (PARTITION BY c.Name ORDER BY c.CreatedDate DESC) AS RowNum
        FROM ClubUsers cu
        JOIN Clubs c ON cu.ClubId = c.ClubId
        WHERE cu.UserId = @UserId
    )
    -- Select only the first row (RowNum = 1) for each unique club name
    SELECT 
        ClubId, 
        Name, 
        clubdetails, 
        IsMemberIDRequired, 
        IsEnableOnSignOn, 
        createddate, 
        ModifiedDate
    FROM UniqueClubs
    WHERE RowNum = 1;
END;

ALTER PROC GetAvailableGroups(
 @UserId INT
)
	 AS	
	BEGIN
		SELECT DISTINCT    
		c.ClubId, 
		c.Name, 
		c.clubdetails, 
		c.IsMemberIDRequired, 
		c.IsEnableOnSignOn, 
		c.createddate, 
		c.ModifiedDate 
 
		From ClubUsers cu
		JOIN clubs c on cu.ClubId = c.ClubId
		Where NOT UserId = @UserId
	END
	----- 

	
	ALTER PROCEDURE GetAvailableGroups
(
    @UserId INT
)
AS
BEGIN
    -- Use a Common Table Expression (CTE) to identify unique clubs based on Name and clubdetails
    WITH UniqueClubs AS (
        SELECT 
            c.ClubId, 
            c.Name, 
            c.clubdetails, 
            c.IsMemberIDRequired, 
            c.IsEnableOnSignOn, 
            c.createddate, 
            c.ModifiedDate,
            ROW_NUMBER() OVER (PARTITION BY c.Name, c.clubdetails ORDER BY c.CreatedDate DESC) AS RowNum
        FROM Clubs c
        WHERE NOT EXISTS (
            SELECT 1 
            FROM ClubUsers cu
            WHERE cu.ClubId = c.ClubId AND cu.UserId = @UserId
        )
    )
    -- Select only the first row (RowNum = 1) for each unique Name and clubdetails
    SELECT 
        ClubId, 
        Name, 
        clubdetails, 
        IsMemberIDRequired, 
        IsEnableOnSignOn, 
        createddate, 
        ModifiedDate
    FROM UniqueClubs
    WHERE RowNum = 1;
END;



	----- unique userId only
CREATE PROC AddAvailableGroups(
 @UserId INT,
 @ClubId INT
)
	AS
		BEGIN
			IF NOT EXISTS(
			  SELECT 1
			  FROM ClubUsers
			  WHERE ClubId=@ClubId AND UserId = @UserId
			)
				BEGIN
				INSERT INTO ClubUsers(ClubId,UserId,CreatedDate)
			SELECT
				@ClubId AS ClubID,
				USERDETAILID,
				GETDATE()
			FROM UserDetails WHERE UserDetailId = @UserId
				END
			
			    
			
			
		END




		----- unique groupNames
ALTER PROC GetAvailableGroups
(
    @UserId INT
)
AS
BEGIN
    WITH RankedClubs AS 
    (
        SELECT 
            c.ClubId, 
            c.Name, 
            c.clubdetails, 
            c.IsMemberIDRequired, 
            c.IsEnableOnSignOn, 
            c.createddate, 
            c.ModifiedDate,
            ROW_NUMBER() OVER (PARTITION BY c.Name ORDER BY c.CreatedDate DESC) AS RowNum
        FROM 
            ClubUsers cu
		
		
        JOIN 
            Clubs c 
        ON 
            cu.ClubId = c.ClubId
        WHERE 
          NOT  cu.UserId = @UserId
    )



	ALTER PROC GetAvailableGroups
(
    @UserId INT
)
AS
BEGIN
    WITH RankedClubs AS 
    (
        SELECT 
            c.ClubId, 
            c.Name, 
            c.clubdetails, 
            c.IsMemberIDRequired, 
            c.IsEnableOnSignOn, 
            c.createddate, 
            c.ModifiedDate,
            ROW_NUMBER() OVER (PARTITION BY c.Name ORDER BY c.CreatedDate DESC) AS RowNum
        FROM 
            Clubs c
        WHERE 
            NOT EXISTS 
            (
                SELECT 1 
                FROM ClubUsers cu
				JOIN Clubs  cc ON cu.ClubId = cc.ClubId
				Where cc.Name = c.Name and cc.ClubDetails = c.ClubDetails
            )
    )
    SELECT 
        ClubId, 
        Name, 
        clubdetails, 
        IsMemberIDRequired, 
        IsEnableOnSignOn, 
        createddate, 
        ModifiedDate
    FROM 
        RankedClubs
    WHERE 
        RowNum = 1; -- Select only the first record for each unique name
END

    SELECT 
        ClubId, 
        Name, 
        clubdetails, 
        IsMemberIDRequired, 
        IsEnableOnSignOn, 
        createddate, 
        ModifiedDate
    FROM 
        RankedClubs
    WHERE 
        RowNum = 1; -- Select only the first record for each unique name
END

CREATE PROCEDURE Fetch_Clubs_By_UserId
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.ClubId, c.Name, c.ClubDetails, c.IsMemberIDRequired, c.IsEnableOnSignOn, c.CreatedDate, c.ModifiedDate
    FROM ClubUsers cu
    JOIN Clubs c ON cu.ClubId = c.ClubId
    WHERE cu.UserId = @UserId;
END;



exec GetUserGroups @UserId

select * from dbo.clubs where ClubId =152

SELECT * FROM ClubUsers WHERE ClubId = 156

select * from ClubUsers WHERE UserId = 3481
select * from Clubs where ClubId = 99

select * from UserDetails

exec PROC_CUSTOM_GET_CLUBS_LIST
exec PROC_CUSTOM_GET_CLUB_USERS_LIST



SELECT ClubId,UserId,CreatedDate FROM ClubUsers;





SELECT USERDETAILID, Email, SignUpDate, ClientStoreId
FROM UserDetails
WHERE SignUpDate >= '2024-11-07' AND SignUpDate <= '2024-11-08';



EXEC Create_Shopper_Groups @SIGNFROMDATE='2024-11-08',@SIGNUPTODATE='2024-11-08',@FIRSTNAME='Jose',@LASTNAME='Ochoa',@USERNAME='jedition6@gmail.com',@ZIPCODE='507322',@STOREID=5,@MEMBERNUMBER='44372633467',@GroupName='DEMO GROUP NAME',@Description='DEMO GROUP',@UserDetailId=3462

EXEC Create_Shopper_Groups @SIGNFROMDATE='2024-11-07',@SIGNUPTODATE='2024-11-07',@FIRSTNAME='Sai',@LASTNAME='Kumar',@USERNAME='sai@gmail.com',@ZIPCODE='507134',@STOREID=4,@MEMBERNUMBER='44205593556',@GroupName='DEMO GROUP NAME',@Description='DEMO GROUP',@UserDetailId=3461
------- Create_Shopper_Groups Is Updated working  ---- 
ALTER PROCEDURE Create_Shopper_Groups
    @SIGNFROMDATE VARCHAR(10),
    @SIGNUPTODATE VARCHAR(10),
    @FIRSTNAME VARCHAR(50),
    @LASTNAME VARCHAR(50),
    @USERNAME VARCHAR(50),
    @ZIPCODE VARCHAR(20),
    @STOREID INT,
    @MEMBERNUMBER VARCHAR(50),
    @GroupName VARCHAR(100),
	@Description VARCHAR(100),
	@UserDetailId INT
AS
BEGIN
    DECLARE @NewGroupID INT = 0
   

    
    INSERT INTO CLUBS (Name, clubdetails, IsMemberIDRequired, IsEnableOnSignOn, createddate, ModifiedDate)
    VALUES (@GroupName, @Description, 0, 0, GETDATE(), GETDATE())

    
    SELECT @NewGroupID = SCOPE_IDENTITY()

   
    IF @NewGroupID > 0
    BEGIN
        
        IF(LEN(@SIGNFROMDATE) > 5 AND LEN(@SIGNUPTODATE) > 5)
        BEGIN
            
            INSERT INTO ClubUsers (ClubId, UserId, CreatedDate)
SELECT @NewGroupID AS ClubID,
       USERDETAILID,  
       GETDATE()
FROM UserDetails 
  WHERE UserDetailId = @UserDetailId
--WHERE IsDeleted = 0
--  AND ( @FIRSTNAME IS NULL OR FirstName LIKE '%' + @FIRSTNAME + '%')
--  AND (@LASTNAME IS NULL OR LastName LIKE '%' + @LASTNAME + '%')
--  AND (@USERNAME IS NULL OR UserName LIKE '%' + @USERNAME + '%')
--  AND (@ZIPCODE IS NULL OR ZipCode LIKE '%' + @ZIPCODE + '%')
--  AND ( @STOREID IS NULL OR @STOREID  = ClientStoreId)
-- --- AND (COALESCE(@STOREID, 0) = 0 OR ClientStoreId = (SELECT ClientStoreId FROM CLIENTSTORES WHERE POSStoreId = @STOREID))
--  AND (@MEMBERNUMBER IS NULL OR BarCodeValue LIKE '%' + @MEMBERNUMBER + '%')
--  AND(
--  SignUpDate >= @SIGNFROMDATE AND SignUpDate <= @SIGNUPTODATE
--  )


  --AND (LEN(@SIGNFROMDATE) <= 5 OR SignUpDate BETWEEN CONVERT(DATE, @SIGNFROMDATE) AND CONVERT(DATE, @SIGNUPTODATE))
   --AND (
   --                (@SIGNFROMDATE IS NULL AND @SIGNUPTODATE IS NULL)
   --                OR (@SIGNFROMDATE IS NOT NULL AND @SIGNUPTODATE IS NULL AND SignUpDate >= @SIGNFROMDATE)
   --                OR (@SIGNFROMDATE IS NULL AND @SIGNUPTODATE IS NOT NULL AND SignUpDate <= @SIGNUPTODATE)
   --                OR (SignUpDate BETWEEN CONVERT(DATE, @SIGNFROMDATE) AND CONVERT(DATE, @SIGNUPTODATE))
   --           )
	   
        END
        ELSE
        BEGIN
            
          INSERT INTO ClubUsers (ClubId, UserId, CreatedDate)
SELECT @NewGroupID AS ClubID,
       USERDETAILID,  
       GETDATE()
FROM UserDetails 
WHERE UserDetailId = @UserDetailId
--WHERE IsDeleted = 0
--  AND ( @FIRSTNAME IS NULL OR FirstName LIKE '%' + @FIRSTNAME + '%')
--  AND (@LASTNAME IS NULL OR LastName LIKE '%' + @LASTNAME + '%')
--  AND (@USERNAME IS NULL OR UserName LIKE '%' + @USERNAME + '%')
--  AND (@ZIPCODE IS NULL OR ZipCode LIKE '%' + @ZIPCODE + '%')
--  AND ( @STOREID IS NULL OR @STOREID  = ClientStoreId)
-- --- AND (COALESCE(@STOREID, 0) = 0 OR ClientStoreId = (SELECT ClientStoreId FROM CLIENTSTORES WHERE POSStoreId = @STOREID))
--  AND (@MEMBERNUMBER IS NULL OR BarCodeValue LIKE '%' + @MEMBERNUMBER + '%')
-- --- AND (LEN(@SIGNFROMDATE) <= 5 OR SignUpDate BETWEEN CONVERT(DATE, @SIGNFROMDATE) AND CONVERT(DATE, @SIGNUPTODATE))

    END
    
END
END


EXEC [dbo].[Create_Shopper_Group] @SIGNFROMDATE='',@SIGNUPTODATE='',@NEWGROUPID=0, @USERSCOUNT=0,@FIRSTNAME='',@LASTNAME='',@USERNAME='', @GROUPID = 0,@MEMBERNUMBER='',@STOREID =0,@SIGNFROMDATE='',@SIGNUPTODATE='',@ZIPCODE=''
@NEWGROUPID INT = 0,
	@USERSCOUNT INT = 0,
	@FIRSTNAME VARCHAR(100)='',
	@LASTNAME VARCHAR(100)='',
	@USERNAME VARCHAR(100)='',
    @GROUPID INT = 0,
	@MEMBERNUMBER VARCHAR(50)='',
	@STOREID INT = 0,
	@SIGNFROMDATE VARCHAR(30),
	@SIGNUPTODATE VARCHAR(30),
	@ZIPCODE VARCHAR(10)=''

CREATE PROC [dbo].[Create_Shopper_Group]
(
	@NEWGROUPID INT = 0,
	@USERSCOUNT INT = 0,
	@FIRSTNAME VARCHAR(100)='',
	@LASTNAME VARCHAR(100)='',
	@USERNAME VARCHAR(100)='',
    @GROUPID INT = 0,
	@MEMBERNUMBER VARCHAR(50)='',
	@STOREID INT = 0,
	@SIGNFROMDATE VARCHAR(30),
	@SIGNUPTODATE VARCHAR(30),
	@ZIPCODE VARCHAR(10)=''

)
  AS
	BEGIN
		IF(LEN(@SIGNFROMDATE ) > 5 AND LEN(@SIGNUPTODATE)> 5) 
			BEGIN
			  INSERT INTO ClubUsers
			  (ClubId,UserId,CreatedDate)
			SELECT
	          @NewGroupID AS ClubID,
		      USERDETAILID,
		      GETDATE()
	       FROM
		     Userdetails WHERE IsDeleted = 0 AND
			 (@FIRSTNAME ='' OR FirstName LIKE '%' + @FIRSTNAME + '%') AND
			 (@LASTNAME ='' OR LastName LIKE '%' + @LASTNAME + '%') AND
			 (@USERNAME='' OR UserName LIKE '%' + @USERNAME + '%') AND
			 (@ZIPCODE ='' OR ZipCode LIKE '%' + @FIRSTNAME + '%') AND
			 --(@GROUPID=0 OR USERDETAILID IN (SELECT USERID FROM CLUBUSERS WHERE CLUBID=@GROUPID)) AND
			 (@STOREID = 0 OR  ClientStoreId =(SELECT ClientStoreId FROM CLIENTSTORES WHERE POSStoreId = @STOREID ) ) AND
			 (@MEMBERNUMBER = '' OR BarCodeValue LIKE '%' + @MEMBERNUMBER + '%' ) AND
			 (SignUpDate between CONVERT(DATE,@SIGNFROMDATE) AND CONVERT(DATE,@SIGNUPTODATE))




			END
			
		ELSE
			BEGIN
			     INSERT INTO ClubUsers
			  (ClubId,UserId,CreatedDate)
			SELECT
	          @NewGroupID AS ClubID,
		      USERDETAILID,
		      GETDATE()
	       FROM
		     Userdetails WHERE IsDeleted = 0 AND
			 (@FIRSTNAME ='' OR FirstName LIKE '%' + @FIRSTNAME + '%') AND
			 (@LASTNAME ='' OR LastName LIKE '%' + @LASTNAME + '%') AND
			 (@USERNAME='' OR UserName LIKE '%' + @USERNAME + '%') AND
			 (@ZIPCODE ='' OR ZipCode LIKE '%' + @FIRSTNAME + '%') AND
			 --(@GROUPID=0 OR USERDETAILID IN (SELECT USERID FROM CLUBUSERS WHERE CLUBID=@GROUPID)) AND
			 (@STOREID = 0 OR  ClientStoreId =(SELECT ClientStoreId FROM CLIENTSTORES WHERE POSStoreId = @STOREID ) ) AND
			 (@MEMBERNUMBER = '' OR BarCodeValue LIKE '%' + @MEMBERNUMBER + '%' )
			-- (SignUpDate between CONVERT(DATE,@SIGNFROMDATE) AND CONVERT(DATE,@SIGNUPTODATE))

			 END
	END

--ALTER PROCEDURE [dbo].[PROC_CUSTOM_CREATE_SHOPPER_GROUP] 

--(
--    @NEWGROUPID INT = 0,
--	@USERSCOUNT INT = 0,
--	@FIRSTNAME VARCHAR(100)='',
--	@LASTNAME VARCHAR(100)='',
--	@USERNAME VARCHAR(100)='',
--	@GROUPID INT=0,
--	@MEMBERNUMBER VARCHAR(50)='',
--	@STOREID INT=0,
--	@SIGNUPFROMDATE varchar(30),
--	@SIGNUPTODATE varchar(30) ,
--	@ZIPCODE VARCHAR(10)=''
--)
--AS 
--BEGIN

--IF( LEN(@SIGNUPFROMDATE) > 5 AND LEN(@SIGNUPTODATE) > 5)
--	BEGIN
	  
--	    INSERT INTO clubusers
--		(ClubId,UserId,CreatedDate)
--	SELECT
--	    @NewGroupID AS ClubID,
--		USERDETAILID,
--		GETDATE()
--	FROM
--		Userdetails+
--	WHERE  ISDELETED = 0 AND
--		(@FIRSTNAME='' OR FIRSTNAME LIKE '%'+@FIRSTNAME+'%') AND
--		(@LASTNAME='' OR LASTNAME LIKE '%'+@LASTNAME+'%')  AND
--		(@USERNAME='' OR USERNAME LIKE '%'+@USERNAME+'%')  AND
--		(@ZIPCODE='' OR ZIPCODE LIKE '%'+@ZIPCODE+'%')  AND
--		--(@GROUPID=0 OR USERDETAILID IN (SELECT USERID FROM CLUBUSERS WHERE CLUBID=@GROUPID)) AND
--		(@STOREID=0 OR CLIENTSTOREID= (SELECT ClientStoreId FROM CLIENTSTORES WHERE POSStoreId= @STOREID)) AND
--		(@MEMBERNUMBER='' OR BARCODEVALUE LIKE '%'+@MEMBERNUMBER+'%')
--		 AND (SIGNUPDATE Between CONVERT(DATE, @SIGNUPFROMDATE) AND CONVERT(DATE, @SIGNUPTODATE))

--	END
--ELSE
--	BEGIN

	   
--	     INSERT INTO clubusers
--		(ClubId,UserId,CreatedDate)
--	SELECT
--	    @NewGroupID AS ClubID,
--		USERDETAILID,
--		GETDATE()
--	FROM
--		Userdetails
--	WHERE  ISDELETED = 0 AND
--		(@FIRSTNAME='' OR FIRSTNAME LIKE '%'+@FIRSTNAME+'%') AND
--		(@LASTNAME='' OR LASTNAME LIKE '%'+@LASTNAME+'%')  AND
--		(@USERNAME='' OR USERNAME LIKE '%'+@USERNAME+'%')  AND
--		(@ZIPCODE='' OR ZIPCODE LIKE '%'+@ZIPCODE+'%')  AND
--		--(@GROUPID=0 OR USERDETAILID IN (SELECT USERID FROM CLUBUSERS WHERE CLUBID=@GROUPID)) AND
--		(@STOREID=0 OR CLIENTSTOREID= (SELECT ClientStoreId FROM CLIENTSTORES WHERE POSStoreId= @STOREID)) AND
--		(@MEMBERNUMBER='' OR BARCODEVALUE LIKE '%'+@MEMBERNUMBER+'%')
--		-- AND (SIGNUPDATE Between CONVERT(DATE, @SIGNUPFROMDATE) AND CONVERT(DATE, @SIGNUPTODATE))

--	END
----- create pagination PROC

EXEC GetPagedSortedFilteredUserDetails
    @PageNumber = 1, 
    @PageSize = 5,   
    @SortColumn = 'UserDetailId',  
    @SortDirection = 'ASC', 
    @SearchTerm = ''; 

CREATE PROCEDURE GetPagedSortedFilteredUserDetails
    @PageNumber INT,
    @PageSize INT,
    @SortColumn NVARCHAR(50) = 'UserDetailId', -- Default sorting column
    @SortDirection NVARCHAR(4) = 'ASC',        -- 'ASC' or 'DESC'
    @SearchTerm NVARCHAR(100) = NULL           -- Search term for filtering
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare dynamic SQL variable
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @SortOrder NVARCHAR(10);

    -- Determine sort direction (ASC or DESC)
    IF @SortDirection = 'ASC' OR @SortDirection = 'DESC'
        SET @SortOrder = @SortDirection;
    ELSE
        SET @SortOrder = 'ASC';  -- Default to ASC if invalid direction

    -- Calculate the starting and ending rows for paging
    DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize + 1;
    DECLARE @EndRow INT = @PageNumber * @PageSize;

    -- Build the dynamic SQL query to apply filters, sorting, and paging
    SET @SQL = N'SELECT 
                    UserDetailId,
                    Email,
                    BarCodeValue,
                    Mobile,
                    ZipCode,
                    ClientStoreId,
                    FirstName,
                    LastName,
                    SignUpDate
                FROM (
                    SELECT 
                        UserDetailId,
                        Email,
                        BarCodeValue,
                        Mobile,
                        ZipCode,
                        ClientStoreId,
                        FirstName,
                        LastName,
                        SignUpDate,
                        ROW_NUMBER() OVER (ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + @SortOrder + ') AS RowNum
                    FROM UserDetails
                    WHERE 
                        (@SearchTerm IS NULL OR
                         Email LIKE ''%' + @SearchTerm + '%'' OR
                         BarCodeValue LIKE ''%' + @SearchTerm + '%'' OR
                         Mobile LIKE ''%' + @SearchTerm + '%'' OR
                         ZipCode LIKE ''%' + @SearchTerm + '%'' OR
                         FirstName LIKE ''%' + @SearchTerm + '%'' OR
                         LastName LIKE ''%' + @SearchTerm + '%'' OR
                         ClientStoreId LIKE ''%' + @SearchTerm + '%'' OR
                         SignUpDate LIKE ''%' + @SearchTerm + '%'' )
                ) AS UserDetailCTE
                WHERE RowNum BETWEEN @StartRow AND @EndRow;';

    -- Execute the dynamic SQL query to get paged, sorted, filtered results
    EXEC sp_executesql @SQL, 
        N'@SearchTerm NVARCHAR(100), @StartRow INT, @EndRow INT',
        @SearchTerm, @StartRow, @EndRow;

    -- Query to get the total count of records for pagination
    SET @SQL = N'SELECT COUNT(*) AS TotalRecords
                FROM UserDetails
                WHERE 
                    (@SearchTerm IS NULL OR
                     Email LIKE ''%' + @SearchTerm + '%'' OR
                     BarCodeValue LIKE ''%' + @SearchTerm + '%'' OR
                     Mobile LIKE ''%' + @SearchTerm + '%'' OR
                     ZipCode LIKE ''%' + @SearchTerm + '%'' OR
                     FirstName LIKE ''%' + @SearchTerm + '%'' OR
                     LastName LIKE ''%' + @SearchTerm + '%'' OR
                     ClientStoreId LIKE ''%' + @SearchTerm + '%'' OR
                     SignUpDate LIKE ''%' + @SearchTerm + '%'' );';

    -- Execute the query to get total records
    EXEC sp_executesql @SQL, 
        N'@SearchTerm NVARCHAR(100)', 
        @SearchTerm;

END;


EXEC pagination_data
    @Page = 2,
    @Limit = 5,
    @ClientStoreId = '6',
 
    @SortDirection = 'DESC';


	---- pagination data 

CREATE PROCEDURE pagination_data
    @Page INT = 1,
    @Limit INT = 5,
    @Email VARCHAR(100) = NULL,
    @BarCodeValue VARCHAR(100) = NULL,
    @Mobile VARCHAR(100) = NULL,
    @ZipCode VARCHAR(100) = NULL,
    @ClientStoreId INT = NULL,
    @FirstName VARCHAR(100) = NULL,
    @LastName VARCHAR(100) = NULL,
    @SignUpFromDate DATE = NULL,
    @SignUpToDate DATE = NULL,
    @SortColumn VARCHAR(100) = 'UserDetailId',  -- Default column to sort by
    @SortDirection VARCHAR(10) = 'ASC'  -- Default sorting direction
AS
BEGIN
    -- Declare the starting row number based on the current page
    DECLARE @Offset INT = (@Page - 1) * @Limit;

    -- Construct the dynamic SQL for sorting direction
    DECLARE @SortOrder VARCHAR(10) = 
        CASE 
            WHEN @SortDirection = 'ASC' THEN 'ASC'
            WHEN @SortDirection = 'DESC' THEN 'DESC'
            ELSE 'ASC'
        END;

    -- Main query for paginated results with dynamic sorting and filtering
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = N'SELECT 
                    UserDetailId,
                    Email,
                    BarCodeValue,
                    Mobile,
                    ZipCode,
                    ClientStoreId,
                    FirstName,
                    LastName,
                    SignUpDate
                FROM UserDetails
                WHERE
                    (@Email IS NULL OR Email LIKE ''%'' + @Email + ''%'')
                    AND (@BarCodeValue IS NULL OR BarCodeValue LIKE ''%'' + @BarCodeValue + ''%'')
                    AND (@Mobile IS NULL OR Mobile LIKE ''%'' + @Mobile + ''%'')
                    AND (@ZipCode IS NULL OR ZipCode LIKE ''%'' + @ZipCode + ''%'')
                    AND (@ClientStoreId IS NULL OR ClientStoreId = @ClientStoreId)
                    AND (@FirstName IS NULL OR FirstName LIKE ''%'' + @FirstName + ''%'')
                    AND (@LastName IS NULL OR LastName LIKE ''%'' + @LastName + ''%'')
                    AND (
                        (@SignUpFromDate IS NULL AND @SignUpToDate IS NULL) 
                        OR (@SignUpFromDate IS NOT NULL AND @SignUpToDate IS NULL AND SignUpDate >= @SignUpFromDate)
                        OR (@SignUpFromDate IS NULL AND @SignUpToDate IS NOT NULL AND SignUpDate <= @SignUpToDate)
                        OR (SignUpDate BETWEEN @SignUpFromDate AND @SignUpToDate)
                    )
                ORDER BY ' + @SortColumn + ' ' + @SortOrder + '
                OFFSET @Offset ROWS
                FETCH NEXT @Limit ROWS ONLY;';

    -- Execute the dynamic SQL for paginated results
    EXEC sp_executesql @SQL, 
        N'@Email VARCHAR(100), @BarCodeValue VARCHAR(100), @Mobile VARCHAR(100), @ZipCode VARCHAR(100),
          @ClientStoreId INT, @FirstName VARCHAR(100), @LastName VARCHAR(100), @SignUpFromDate DATE, 
          @SignUpToDate DATE, @Offset INT, @Limit INT',
        @Email, @BarCodeValue, @Mobile, @ZipCode, @ClientStoreId, @FirstName, @LastName, 
        @SignUpFromDate, @SignUpToDate, @Offset, @Limit;

    -- Query for TotalRecords and TotalPages (no pagination applied)
    SELECT 
        COUNT(UserDetailId) AS TotalRecords, 
        CAST(CEILING((COUNT(UserDetailId) * 1.0) / @Limit) AS INT) AS TotalPages
    FROM UserDetails
    WHERE
        (@Email IS NULL OR Email LIKE '%' + @Email + '%')
        AND (@BarCodeValue IS NULL OR BarCodeValue LIKE '%' + @BarCodeValue + '%')
        AND (@Mobile IS NULL OR Mobile LIKE '%' + @Mobile + '%')
        AND (@ZipCode IS NULL OR ZipCode LIKE '%' + @ZipCode + '%')
        AND (@ClientStoreId IS NULL OR ClientStoreId = @ClientStoreId)
        AND (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%')
        AND (@LastName IS NULL OR LastName LIKE '%' + @LastName + '%')
        AND (
            (@SignUpFromDate IS NULL AND @SignUpToDate IS NULL)
            OR (@SignUpFromDate IS NOT NULL AND @SignUpToDate IS NULL AND SignUpDate >= @SignUpFromDate)
            OR (@SignUpFromDate IS NULL AND @SignUpToDate IS NOT NULL AND SignUpDate <= @SignUpToDate)
            OR (SignUpDate BETWEEN @SignUpFromDate AND @SignUpToDate)
        );
END;


SELECT * FROM UserDetails where UserDetailId = 3461 AND BarCodeValue ='44205593556';
SELECT *  FROM ClientStores


------ Add points : rewardtype, membernumber, transaction amount

SELECT * FROM BasketData Where BasketDataId = 97221
select * from aspnet_Membership
select * from UserDetails
select * from UserDeviceInfo

SELECT * FROM UserDetails
SELECT * FROM LM_USER_REWARD
SELECT * FROM UserTypes
SELECT * FROM SSNews where NEWSID=480

exec PROC_CUSTOM_GET_USER_CLIPS_REDEMPTION_DATES @USERID=3461
SELECT * FROM SSNEWSUSERSNCRIMPRESSIONS where UserId=2460

---- UPDATE SSNEWSUSERSNCRIMPRESSIONS SET UserId = 3461 where NewsId=1699  edit
----UPDATE SSNEWSUSERSNCRIMPRESSIONS SET UserId = 2460 where NewsId=1699   old
SELECT * FROM UserDetails
SELECT * FROM LM_USER_REWARD

exec dbo.GetUserDetails @UserId=1
UPDATE UserDetails SET 
ClientStoreId=6
where UserDetailId = 3474

UPDATE UserDetails SET 
Email = 'dummytest12@icloud.com',
CustomerID=7,
ClientStoreId=6,
BarCodeValue='44170681221',
SignUpDate = GETDATE()
where UserDetailId = 3478




select * from BasketData order by BasketDataId desc
select
SELECT * FROM BasketItems order by Amount desc
SELECT * FROM BasketItems where BasketDataId=97228; 
SELECT * FROM BasketConsumerIds where BasketDataId=97228; 

EXEC PROC_CUSTOM_SAVE_USER_POINTS @MemberNumber='44205593556',@UPC1='5',@UPC2='1234',@QTY1='1',@QTY2='2',@TransactionTotalAmount='1094',

EXEC SAVE_USER_POINTS @MemberNumber='44205593556',@UPC1='',@UPC2='',@QTY1='',@QTY2='',@TransactionTotalAmount='400',@Type='2'

 --------------  save points proc  ----

 SELECT * FROM BasketData;
SELECT * FROM BasketItems;
SELECT * FROM BasketConsumerIds;

CREATE PROC SAVE_USER_POINTS(
@MemberNumber VARCHAR(15),
@UPC1 VARCHAR(20),
@UPC2 VARCHAR(20),
@QTY1 INT,
@QTY2 INT,
@TransactionTotalAmount INT,
@Type INT
)
AS
	BEGIN
			SET NOCOUNT ON;
		     DECLARE @BasketDataId INT ='';
			 IF(@Type = 1)
				BEGIN
					INSERT INTO BasketData(
					 Retailer,
					 POSId,
					 StoreId,
					 OperatorId,
					 TransactionId,
					 TransactionDate,
					 TransactionTime,
					 TransactionTotalAmount,
					 TransactionTaxAmount,
					 TransactionTenderType,
					 CreatedDate,
					 IsProcessed,
					 BasketGUID,
					 IsUPCSplitComplete,
					 IsProcessedForMfg

					)
					VALUES(
					'ALLIANCE_POINTS',
					4,
					1,
					133,
					365354,
					GETDATE(),
					
				   '10:59:19.0000000',
					@TransactionTotalAmount,
					0,
					'CASH',
					GETDATE(),
					0,
					NEWID(),
					0,
					0
					)
					
					SET @BasketDataId = SCOPE_IDENTITY();
					INSERT INTO BasketItems(
					BasketDataId,
					UPC,
					IdType,
					Amount,
					DeptId,
					Qty,
					QtyType,
					SaleType,
					CoPrefix,
					FamilyCode1,
					FamilyCode2,
					CreatedDate						  

					)
					VALUES(
					 @BasketDataId,

							 @UPC1,

							 'GTIN-12',

							 100,

							 0,

							 @QTY1,

							 'U',

							 'S',

							 '',

							 '100',

							 '',

							 GETDATE()

					)

					IF(LEN(@UPC2) > 3 AND @QTY2 > 0)
						BEGIN
							 INSERT INTO BasketItems(

									   BasketDataId,

									   UPC,

									   IdType,

									   Amount,

									   DeptId,

									   Qty,

									   QtyType,

									   SaleType,

									   CoPrefix,

									   FamilyCode1,

									   FamilyCode2,

									   CreatedDate

									   ) 

							VALUES

									   (

										 @BasketDataId,

										 @UPC2,

										 'GTIN-12',

										 100,

										 0,

										 @QTY2,

										 'U',

										 'S',

										 '',

										 '100',

										 '',

										 GETDATE()

									   )
						END
                   INSERT INTO BasketConsumerIds(
					BasketDataId,
					LoyaltyId,
					MobileNumber,
					EmailAddress,
					AlternateId					  
				   )
				   VALUES(
				    @BasketDataId,
					@MemberNumber,
					'',
					'',
					''
				   )

				END
			ELSE
			    ----- FOR BASKET COUPONS
			   BEGIN
				   INSERT INTO BasketData(
					 Retailer,
					 POSId,
					 StoreId,
					 OperatorId,
					 TransactionId,
					 TransactionDate,
					 TransactionTime,
					 TransactionTotalAmount,
					 TransactionTaxAmount,
					 TransactionTenderType,
					 CreatedDate,
					 IsProcessed,
					 BasketGUID,
					 IsUPCSplitComplete,
					 IsProcessedForMfg

					)
					VALUES(
					'ALLIANCE_POINTS',
					4,
					1,
					133,
					365354,
					GETDATE(),
					
				   '10:59:19.0000000',
					@TransactionTotalAmount,
					0,
					'CASH',
					GETDATE(),
					0,
					NEWID(),
					0,
					0
					)
					
					SET @BasketDataId = SCOPE_IDENTITY();
					INSERT INTO BasketConsumerIds(
					BasketDataId,
					LoyaltyId,
					MobileNumber,
					EmailAddress,
					AlternateId					  
				   )
				   VALUES(
				    @BasketDataId,
					@MemberNumber,
					'',
					'',
					''
				   )
			   END

		
	END

 ---------- ==== END PROC
 --- veritra veritra5g
  ---- Hyder@b@d#ver!tr@


SELECT * FROM dbo.LM_REWARDS
INSERT INTO [dbo].[LM_REWARDS] 
(
ValidFrom,
ExpiresOn,
Title,
RewardTypeID,
BuyQtyAmount,
RewardQtyAmount,
RewardTitle,
AdditionalDetails,
POSDetails,
ImageURL,
CreatedUserID,
CreatedDateTime,
RewardGroupID,
CouponID,
RewardStatus,
RewardDepartmentID,
RewardMustBeUsedByDate,
IsTargetSpecific,
IsDiscountPercentage,
RewardCouponMinQty,
RewardCouponTypeID,
RewardQtyAmountMoney,
IsDepartmentSpecific,
IsStoreSpecific,
PointsPerEach,
IsPointsBased,
TierValue,
NumberOfVisits

)
VALUES(


'2024-09-30', 
    '2024-10-30', 
    'Earn rewards on every visit! Spend $400 get $5 back!', 
    2, 
    400,  
    5,  
    '$5 off your NEXT visit',  
    '',
    'Loyalty Reward',
   'https://www.google.com/url?sa=i&url=https%3A%2F%2Fyourdailydeals.co.uk%2Floyalty-in-shopping-how-reward-programs-can-boost-your-savings%2F&psig=AOvVaw0W7i1JOANby8RLhRRBFNR8&ust=1727775348898000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCOCujb2u6ogDFQAAAAAdAAAAABAE',

    6,  
    GETDATE(), 
    11, 
    123, 
    1,  
    10,  
    GETDATE(),
    1,  
    10, 
    0,  
    2,  
    100.00, 
    0, 
    0,  
    50.00,
    1, 
    5, 
    10 
)

INSERT INTO [dbo].[LM_REWARDS] 
(
ValidFrom,
ExpiresOn,
Title,
RewardTypeID,
BuyQtyAmount,
RewardQtyAmount,
RewardTitle,
AdditionalDetails,
POSDetails,
ImageURL,
CreatedUserID,
CreatedDateTime,
RewardGroupID,
CouponID,
RewardStatus,
RewardDepartmentID,
RewardMustBeUsedByDate,
IsTargetSpecific,
IsDiscountPercentage,
RewardCouponMinQty,
RewardCouponTypeID,
RewardQtyAmountMoney,
IsDepartmentSpecific,
IsStoreSpecific,
PointsPerEach,
IsPointsBased,
TierValue,
NumberOfVisits

)
VALUES(


'2024-10-30', 
    '2024-11-30', 
    'Buy 10 Gallons of Best Choice White Milk Get 11th FREE', 
    1, 
    10,  
    1,  
    'FREE Gallon of Milk on NEXT visit',  
    '',
    'Milk Rewards',
   'https://www.google.com/url?sa=i&url=https%3A%2F%2Fyourdailydeals.co.uk%2Floyalty-in-shopping-how-reward-programs-can-boost-your-savings%2F&psig=AOvVaw0W7i1JOANby8RLhRRBFNR8&ust=1727775348898000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCOCujb2u6ogDFQAAAAAdAAAAABAE',

    6,  
    GETDATE(), 
    12, 
    124, 
    2,  
    11,  
    GETDATE(),
    2,  
    10, 
    0,  
    2,  
    120.00, 
    0, 
    0,  
    60.00,
    2, 
    5, 
    5 
)




INSERT INTO [dbo].[LM_REWARDS] 
(
ValidFrom,
ExpiresOn,
Title,
RewardTypeID,
BuyQtyAmount,
RewardQtyAmount,
RewardTitle,
AdditionalDetails,
POSDetails,
ImageURL,
CreatedUserID,
CreatedDateTime,
RewardGroupID,
CouponID,
RewardStatus,
RewardDepartmentID,
RewardMustBeUsedByDate,
IsTargetSpecific,
IsDiscountPercentage,
RewardCouponMinQty,
RewardCouponTypeID,
RewardQtyAmountMoney,
IsDepartmentSpecific,
IsStoreSpecific,
PointsPerEach,
IsPointsBased,
TierValue,
NumberOfVisits

)
VALUES(


'2024-10-30', 
    '2024-11-30', 
    'Buy 10 Gallons of Best Choice White Milk Get 11th FREE', 
    1, 
    10,  
    1,  
    'FREE Gallon of Milk on NEXT visit',  
    '',
    'Milk Rewards',
   'https://www.google.com/url?sa=i&url=https%3A%2F%2Fyourdailydeals.co.uk%2Floyalty-in-shopping-how-reward-programs-can-boost-your-savings%2F&psig=AOvVaw0W7i1JOANby8RLhRRBFNR8&ust=1727775348898000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCOCujb2u6ogDFQAAAAAdAAAAABAE',

    6,  
    GETDATE(), 
    12, 
    124, 
    2,  
    11,  
    GETDATE(),
    2,  
    10, 
    0,  
    2,  
    120.00, 
    0, 
    0,  
    60.00,
    2, 
    5, 
    5 
)


INSERT INTO [dbo].[LM_REWARDS] 
(
ValidFrom,
ExpiresOn,
Title,
RewardTypeID,
BuyQtyAmount,
RewardQtyAmount,
RewardTitle,
AdditionalDetails,
POSDetails,
ImageURL,
CreatedUserID,
CreatedDateTime,
RewardGroupID,
CouponID,
RewardStatus,
RewardDepartmentID,
RewardMustBeUsedByDate,
IsTargetSpecific,
IsDiscountPercentage,
RewardCouponMinQty,
RewardCouponTypeID,
RewardQtyAmountMoney,
IsDepartmentSpecific,
IsStoreSpecific,
PointsPerEach,
IsPointsBased,
TierValue,
NumberOfVisits

)
VALUES(


'2024-08-30', 
    '2024-12-30', 
    'Buy 10 1/2 gallons of Best Choice white milk get 11th FREE', 
    1, 
    10,  
    1,  
    'FREE 1/2 gallon of Best Choice white milk on NEXT visitE',  
    '',
    'Milk Rewards',
   'https://www.google.com/url?sa=i&url=https%3A%2F%2Fyourdailydeals.co.uk%2Floyalty-in-shopping-how-reward-programs-can-boost-your-savings%2F&psig=AOvVaw0W7i1JOANby8RLhRRBFNR8&ust=1727775348898000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCOCujb2u6ogDFQAAAAAdAAAAABAE',

    6,  
    GETDATE(), 
    0, 
    124, 
    2,  
    11,  
    GETDATE(),
    2,  
    10, 
    0,  
    2,  
    120.00, 
    0, 
    0,  
    60.00,
    2, 
    5, 
    5 
)

UPDATE LM_REWARDS SET ImageURL = 'https://www.unitedsupermarkets.com/Media/UnitedSuperMarkets/Images/UNFM-40919-Rewards-Creative-Refresh---Perk-Bubbles_Groceries.png' WHERE  LM_REWARD_ID =1
UPDATE LM_REWARDS SET ImageURL = 'https://thumbs.dreamstime.com/z/reward-grunge-vintage-stamp-isolated-white-background-reward-sign-reward-stamp-148406856.jpg' WHERE LM_REWARD_ID =2
UPDATE LM_REWARDS SET ImageURL = 'https://th.bing.com/th/id/OIP.yqJdCVv4X3wEjIIqHEYEpwHaFV?rs=1&pid=ImgDetMain' WHERE LM_REWARD_ID =3



CREATE PROC GET_LM_REWARD

AS
	BEGIN 
	SELECT ImageURL,Title,RewardTitle,IsPointsBased,ValidFrom,ExpiresOn FROM LM_REWARDS
	END

exec GET_LM_REWARD

select * from LM_REWARDS

CREATE PROCEDURE GetPagedSortedFilteredUserDetails
    @PageNumber INT,
    @PageSize INT,
    @SortColumn NVARCHAR(50) = 'UserDetailId', -- Default sorting column
    @SortDirection NVARCHAR(4) = 'ASC',        -- 'ASC' or 'DESC'
    @SearchTerm NVARCHAR(100) = NULL           -- Search term for filtering
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare dynamic SQL variable
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @SortOrder NVARCHAR(10);

    -- Determine sort direction (ASC or DESC)
    IF @SortDirection = 'ASC' OR @SortDirection = 'DESC'
        SET @SortOrder = @SortDirection;
    ELSE
        SET @SortOrder = 'ASC';  -- Default to ASC if invalid direction

    -- Calculate the starting and ending rows for paging
    DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize + 1;
    DECLARE @EndRow INT = @PageNumber * @PageSize;

    -- Build the dynamic SQL query to apply filters, sorting, and paging
    SET @SQL = N'SELECT 
                    UserDetailId,
                    Email,
                    BarCodeValue,
                    Mobile,
                    ZipCode,
                    ClientStoreId,
                    FirstName,
                    LastName,
                    SignUpDate
                FROM (
                    SELECT 
                        UserDetailId,
                        Email,
                        BarCodeValue,
                        Mobile,
                        ZipCode,
                        ClientStoreId,
                        FirstName,
                        LastName,
                        SignUpDate,
                        ROW_NUMBER() OVER (ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + @SortOrder + ') AS RowNum
                    FROM UserDetails
                    WHERE 
                        (@SearchTerm IS NULL OR
                         Email LIKE ''%' + @SearchTerm + '%'' OR
                         BarCodeValue LIKE ''%' + @SearchTerm + '%'' OR
                         Mobile LIKE ''%' + @SearchTerm + '%'' OR
                         ZipCode LIKE ''%' + @SearchTerm + '%'' OR
                         FirstName LIKE ''%' + @SearchTerm + '%'' OR
                         LastName LIKE ''%' + @SearchTerm + '%'' )
                ) AS UserDetailCTE
                WHERE RowNum BETWEEN @StartRow AND @EndRow;';

    -- Execute the dynamic SQL query to get paged, sorted, filtered results
    EXEC sp_executesql @SQL, 
        N'@SearchTerm NVARCHAR(100), @StartRow INT, @EndRow INT',
        @SearchTerm, @StartRow, @EndRow;

    -- Query to get the total count of records for pagination
    SET @SQL = N'SELECT COUNT(*) AS TotalRecords
                FROM UserDetails
                WHERE 
                    (@SearchTerm IS NULL OR
                     Email LIKE ''%' + @SearchTerm + '%'' OR
                     BarCodeValue LIKE ''%' + @SearchTerm + '%'' OR
                     Mobile LIKE ''%' + @SearchTerm + '%'' OR
                     ZipCode LIKE ''%' + @SearchTerm + '%'' OR
                     FirstName LIKE ''%' + @SearchTerm + '%'' OR
                     LastName LIKE ''%' + @SearchTerm + '%'' );';

    -- Execute the query to get total records
    EXEC sp_executesql @SQL, 
        N'@SearchTerm NVARCHAR(100)', 
        @SearchTerm;

END;


CREATE PROCEDURE search_records_without_limit
    @Email VARCHAR(100) = NULL,
    @BarCodeValue VARCHAR(100) = NULL,
    @Mobile VARCHAR(100) = NULL,
    @ZipCode VARCHAR(100) = NULL,
    @ClientStoreId INT = NULL,
    @FirstName VARCHAR(100) = NULL,
    @LastName VARCHAR(100) = NULL,
    @SignUpFromDate DATE = NULL,
    @SignUpToDate DATE = NULL,
    @SortColumn VARCHAR(100) = 'UserDetailId', -- Default column to sort by
    @SortDirection VARCHAR(10) = 'ASC'          -- Default sorting direction
AS
BEGIN
    -- Default Limit (records per page)
    DECLARE @Limit INT = 5;  -- Default to 10 records per page if not passed.

    -- Set the sort direction based on input
    DECLARE @SortOrder VARCHAR(10) = 
        CASE 
            WHEN @SortDirection = 'ASC' THEN 'ASC'
            WHEN @SortDirection = 'DESC' THEN 'DESC'
            ELSE 'ASC'
        END;

    -- Query to get all records matching the search criteria (no pagination)
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = N'SELECT 
                    UserDetailId,
                    Email,
                    BarCodeValue,
                    Mobile,
                    ZipCode,
                    ClientStoreId,
                    FirstName,
                    LastName,
                    SignUpDate
                FROM UserDetails
                WHERE
                    (@Email IS NULL OR Email LIKE ''%'' + @Email + ''%'')
                    AND (@BarCodeValue IS NULL OR BarCodeValue LIKE ''%'' + @BarCodeValue + ''%'')
                    AND (@Mobile IS NULL OR Mobile LIKE ''%'' + @Mobile + ''%'')
                    AND (@ZipCode IS NULL OR ZipCode LIKE ''%'' + @ZipCode + ''%'')
                    AND (@ClientStoreId IS NULL OR ClientStoreId = @ClientStoreId)
                    AND (@FirstName IS NULL OR FirstName LIKE ''%'' + @FirstName + ''%'')
                    AND (@LastName IS NULL OR LastName LIKE ''%'' + @LastName + ''%'')
                    AND (
                        (@SignUpFromDate IS NULL AND @SignUpToDate IS NULL) 
                        OR (@SignUpFromDate IS NOT NULL AND @SignUpToDate IS NULL AND SignUpDate >= @SignUpFromDate)
                        OR (@SignUpFromDate IS NULL AND @SignUpToDate IS NOT NULL AND SignUpDate <= @SignUpToDate)
                        OR (SignUpDate BETWEEN @SignUpFromDate AND @SignUpToDate)
                    )
                ORDER BY ' + @SortColumn + ' ' + @SortOrder + ';';

    -- Execute the dynamic SQL for matching records
    EXEC sp_executesql @SQL, 
        N'@Email VARCHAR(100), @BarCodeValue VARCHAR(100), @Mobile VARCHAR(100), @ZipCode VARCHAR(100),
          @ClientStoreId INT, @FirstName VARCHAR(100), @LastName VARCHAR(100), @SignUpFromDate DATE, 
          @SignUpToDate DATE',
        @Email, @BarCodeValue, @Mobile, @ZipCode, @ClientStoreId, @FirstName, @LastName, 
        @SignUpFromDate, @SignUpToDate;

    -- Query to calculate TotalRecords and TotalPages (based on the default Limit value)
    SELECT 
        COUNT(UserDetailId) AS TotalRecords,
        CAST(CEILING(COUNT(UserDetailId) * 1.0 / @Limit) AS INT) AS TotalPages
    FROM UserDetails
    WHERE
        (@Email IS NULL OR Email LIKE '%' + @Email + '%')
        AND (@BarCodeValue IS NULL OR BarCodeValue LIKE '%' + @BarCodeValue + '%')
        AND (@Mobile IS NULL OR Mobile LIKE '%' + @Mobile + '%')
        AND (@ZipCode IS NULL OR ZipCode LIKE '%' + @ZipCode + '%')
        AND (@ClientStoreId IS NULL OR ClientStoreId = @ClientStoreId)
        AND (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%')
        AND (@LastName IS NULL OR LastName LIKE '%' + @LastName + '%')
        AND (
            (@SignUpFromDate IS NULL AND @SignUpToDate IS NULL)
            OR (@SignUpFromDate IS NOT NULL AND @SignUpToDate IS NULL AND SignUpDate >= @SignUpFromDate)
            OR (@SignUpFromDate IS NULL AND @SignUpToDate IS NOT NULL AND SignUpDate <= @SignUpToDate)
            OR (SignUpDate BETWEEN @SignUpFromDate AND @SignUpToDate)
        );
END;


exec  search_records_without_limit

@ClientStoreId = 6



ALTER PROCEDURE Create_Shopper_Groups
    @SIGNFROMDATE VARCHAR(10),
    @SIGNUPTODATE VARCHAR(10),
    @FIRSTNAME VARCHAR(50),
    @LASTNAME VARCHAR(50),
    @USERNAME VARCHAR(50),
    @ZIPCODE VARCHAR(20),
    @STOREID INT,
    @MEMBERNUMBER VARCHAR(50),
    @GroupName VARCHAR(100),
	@Description VARCHAR(100)
AS
BEGIN
    DECLARE @NewGroupID INT = 0
   

    
    INSERT INTO CLUBS (Name, clubdetails, IsMemberIDRequired, IsEnableOnSignOn, createddate, ModifiedDate)
    VALUES (@GroupName, @Description, 0, 0, GETDATE(), GETDATE())

    
    SELECT @NewGroupID = SCOPE_IDENTITY()

   
    IF @NewGroupID > 0
    BEGIN
        
        IF(LEN(@SIGNFROMDATE) > 5 AND LEN(@SIGNUPTODATE) > 5)
        BEGIN
            
            INSERT INTO ClubUsers (ClubId, UserId, CreatedDate)
           SELECT @NewGroupID AS ClubID,
                   USERDETAILID,  
                   GETDATE()
            FROM UserDetails 
            WHERE IsDeleted = 0
              AND (@FIRSTNAME = '' OR FirstName LIKE '%' + @FIRSTNAME + '%')
              AND (@LASTNAME = '' OR LastName LIKE '%' + @LASTNAME + '%')
              AND (@USERNAME = '' OR UserName LIKE '%' + @USERNAME + '%')
              AND (@ZIPCODE = '' OR ZipCode LIKE '%' + @ZIPCODE + '%')
              AND (@STOREID = 0 OR ClientStoreId = (SELECT ClientStoreId FROM CLIENTSTORES WHERE POSStoreId = @STOREID))
              AND (@MEMBERNUMBER = '' OR BarCodeValue LIKE '%' + @MEMBERNUMBER + '%')
              AND SignUpDate BETWEEN CONVERT(DATE, @SIGNFROMDATE) AND CONVERT(DATE, @SIGNUPTODATE)
        END
        ELSE
        BEGIN
            
            INSERT INTO ClubUsers (ClubId, UserId, CreatedDate)
            SELECT @NewGroupID AS ClubID,
                   USERDETAILID,  
                   GETDATE()
            FROM UserDetails 
            WHERE IsDeleted = 0
              AND (@FIRSTNAME = '' OR FirstName LIKE '%' + @FIRSTNAME + '%')
              AND (@LASTNAME = '' OR LastName LIKE '%' + @LASTNAME + '%')
              AND (@USERNAME = '' OR UserName LIKE '%' + @USERNAME + '%')
              AND (@ZIPCODE = '' OR ZipCode LIKE '%' + @ZIPCODE + '%')
              AND (@STOREID = 0 OR ClientStoreId = (SELECT ClientStoreId FROM CLIENTSTORES WHERE POSStoreId = @STOREID))
              AND (@MEMBERNUMBER = '' OR BarCodeValue LIKE '%' + @MEMBERNUMBER + '%')
    END 
    
END
END 