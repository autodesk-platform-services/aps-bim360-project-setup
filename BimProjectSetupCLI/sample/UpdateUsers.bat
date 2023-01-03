set APS_CLIENT_ID=<your_client_id_without_quotation_marks>
set APS_CLIENT_SECRET=<your_client_secret_without_quotation_marks>
set APS_BIM_ACCOUNT_ID=<your_account_id_without_quotation_marks>
cd ..
Autodesk.BimProjectSetup.exe -u ".\sample\BIM360_ProjectUser_UP_Template.csv" -h "admin@company.com" --UP
pause