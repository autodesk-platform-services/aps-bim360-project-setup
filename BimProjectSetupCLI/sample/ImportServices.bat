﻿set APS_CLIENT_ID=<your_client_id_without_quotation_marks>
set APS_CLIENT_SECRET=<your_client_secret_without_quotation_marks>
set APS_BIM_ACCOUNT_ID=<your_account_id_without_quotation_marks>
cd ..
Autodesk.BimProjectSetup.exe -x ".\sample\BIM360_Service_Template.csv"
pause