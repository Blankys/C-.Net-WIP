﻿using log4net;
using System.Collections;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System;
using WebBrowserLaboratory.Helpers;

namespace WebBrowserLaboratory.Forms
{
    public partial class MyWebBrowser : BaseForm
    {
        private string SystemWebsite_Url = ConfigurationManager.AppSettings["SystemWebsite.Url"];
        private string SystemWebsite_UserName = ConfigurationManager.AppSettings["SystemWebsite.UserName"];
        private string SystemWebsite_Password = ConfigurationManager.AppSettings["SystemWebsite.Password"];
        private string DeathByCaptchaService_UserName = ConfigurationManager.AppSettings["DeathByCaptchaService.UserName"];
        private string DeathByCaptchaService_Password = ConfigurationManager.AppSettings["DeathByCaptchaService.Password"];

        private const string WEBBROWSER_TITLE = "Web Browser Laboratory";
        private const string CLICK_EVENT = "Click";
        private const string OPEN_BUTTON = "Open";
        private const string SAVE_BUTTON = "Save";
        private const string CANCEL_BUTTON = "Cancel";
        private const string ENTER_KEY = "{ENTER}";
        private const string DEFAULT_ATTACHMENTS_FOLDER = @"C:\ApptividadAttachments\";

        private static ILog logger;

        public MyWebBrowser()
        {
            Log4Net.Init();
            logger = LogManager.GetLogger(typeof(MyWebBrowser));
            InitializeComponent();
            this.urlTextBox.Text = SystemWebsite_Url;
            this.webBrowser.Navigate(this.urlTextBox.Text);
        }

        private void GoButton_Click(object pSender, EventArgs pEvent)
        {
            try
            {
                DownloadFile();
            }
            catch (Exception vE)
            {
                logger.Error(vE);
                this.JavaScriptAlertMessage(this.webBrowser, vE.GetExceptionMessage());
            }
        }

        private void NavigateButton_Click(object pSender, EventArgs pEvent)
        {
            this.webBrowser.Navigate(this.urlTextBox.Text);
        }

        private void ResolveGoogleCaptcha()
        {
            try
            {
                HtmlHelper.SetValueToElement(this.webBrowser.Document, "UserName", SystemWebsite_UserName);
                HtmlHelper.SetValueToElement(this.webBrowser.Document, "Password", SystemWebsite_Password);

                DeathByCaptcha.Client client = (DeathByCaptcha.Client)new DeathByCaptcha.HttpClient(DeathByCaptchaService_UserName, DeathByCaptchaService_Password);
                DeathByCaptcha.Captcha captcha = client.Decode(
                    DeathByCaptcha.Client.DefaultTokenTimeout,
                    new Hashtable() {
                    { "type", 4 },
                    { "token_params", "{ \"googlekey\": \"" + HtmlHelper.GetGoogleKey(this.webBrowser.DocumentText) + "\", \"pageurl\": \"" + SystemWebsite_Url + "\" }" }
                    }
                );

                HtmlHelper.SetValueToElement(this.webBrowser.Document, "g-recaptcha-response", captcha.Text);
                this.RunActionAndWaitUntilPageLoaded(this.webBrowser, HtmlHelper.ClickElement(this.webBrowser.Document, "action:DoLogin"));
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        private void Fwla()
        {
            try
            {
                #region login
                HtmlHelper.SetValueToElement(this.webBrowser.Document, "UserName", SystemWebsite_UserName);
                HtmlHelper.SetValueToElement(this.webBrowser.Document, "Password", SystemWebsite_Password);
                HtmlHelper.SetValueToElement(this.webBrowser.Document, "captcha", "4vwrs");
                this.RunActionAndWaitUntilPageLoaded(this.webBrowser, HtmlHelper.ClickElement(this.webBrowser.Document, "button_validate"));
                #endregion

                if (this.webBrowser.Url.ToString().Equals("http://localhost:17720/eFWLA/index"))
                {
                    #region inscribir
                    this.RunActionAndWaitUntilPageLoaded(this.webBrowser, HtmlHelper.ClickElement(this.webBrowser.Document, "href", false, "a", "http://localhost:17720/eFWLA/create_contract"));

                    string[] vJavaScriptLibraries = new string[] {
                        "http://localhost:17720/Sources/jquery-2.0.3.min.js",
                        "http://localhost:17720/Sources/jquery.mb.browser.min.js",
                        "http://localhost:17720/Sources/jquery-ui.min.js",
                        "http://localhost:17720/Sources/jquery.ui.datepicker-es.min.js",
                        "http://localhost:17720/Sources/jquery-fluid16.js",
                        "http://localhost:17720/Sources/pqselect.min.js",
                        "http://localhost:17720/Sources/multiselect.min.js",
                        "http://localhost:17720/Sources/jquery.formatCurrency-1.4.0.js",
                        "http://localhost:17720/Sources/jquery.epiclock.min.js",
                        "http://localhost:17720/Sources/jquery.validate.js",
                        "http://localhost:17720/Sources/additional-methods.js",
                        "http://localhost:17720/Sources/jquery.maskedinput.js",
                        "http://localhost:17720/Sources/fwla_validation.js",
                        "http://localhost:17720/Sources/jquery.prettyPhoto.js",
                        "http://localhost:17720/Sources/jquery.form.js",
                        "http://localhost:17720/Sources/jquery.prettyLoader.js",
                        "http://localhost:17720/Sources/jquery.blockUI.js",
                        "http://localhost:17720/Sources/swfobject.js",
                        "http://localhost:17720/Sources/passwordStrengthMeter.js",
                        "http://localhost:17720/Sources/custom.js",
                        "http://localhost:17720/Sources/plupload.full.min.js",
                        "http://localhost:17720/Sources/jquery.plupload.queue.js",
                        "http://localhost:17720/Sources/jquery.ui.plupload.min.js",
                        "http://localhost:17720/Sources/es.js",
                        "http://localhost:17720/Sources/datatables.min.js",
                        "http://localhost:17720/Sources/dataTables.jqueryui.min.js",
                        "http://localhost:17720/Sources/dataTables.colReorder.min.js"
                    };
                    this.ImportJavaScriptLibraries(this.webBrowser, vJavaScriptLibraries);

                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "CBond", "SJNFBA11Z2200602");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "CBondValue", "25,000.00");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "TContractID", "01");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "WarrantyID", "0101");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "CDateStart", "2018-06-04");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "CDateEnd", "2025-06-04");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "CPayStart", "2018-07-10");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "CDisbursementDate", "2018-06-20");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "CPayDay", "10");
                    HtmlHelper.SetValueToElement(this.webBrowser.Document, "CContractType", "P");
                    HtmlHelper.ClickElement(this.webBrowser.Document, "className", true, "input", "open1 nextbutton");
                    #endregion
                }
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        private void DownloadFile()
        {
            try
            {
                string vDownloadDialogTitle = "File Download";
                string vDownloadDialogSaveButton = SAVE_BUTTON;

                string vSaveAsDialogTitle = "Save As";
                string vSaveAsDialogSaveButton = SAVE_BUTTON;
                string vSaveAsDialogFileNameFieldId = "1001";
                string vFileNameToDownload = "RegistroPrevioInactivo_Detalles_DXCBtn0.pdf";

                string vIdentifierElement = "ctl00_MainContent_BuscadorFolios1_FichaFolio1_ASPxPopupControl1_RegistroPrevioInactivo_DetallesASPxGridView_DXCBtn0";

                // Se crea el task que se encargará de manejar el Dialog de subir adjuntos,
                // la cual se ejecutará 5 segundos despues de que se le de click a seleccionar archivo
                Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(500);
                    try
                    {
                        AutomationElement vDesktopObject = AutomationElement.RootElement;

                        #region WebBrowserWindow
                        // Obtener la ventana del robot, por medio del titulo 'RPA'
                        AutomationElement vWebBrowserWindow = vDesktopObject.FindFirst(TreeScope.Children, this.NameProperty(WEBBROWSER_TITLE));
                        #endregion

                        #region DownloadDialog
                        // Obtener el Dialog de "File Download" por medio del título y el nombre de la ventana que lo levantó
                        AutomationElement vDownloadDialogWindow = vDesktopObject.FindFirst(TreeScope.Children, this.NameProperty(vDownloadDialogTitle));

                        // Obtener el Button de "Save"
                        AutomationElement vDownloadDialogSaveButtonElement = vDownloadDialogWindow.FindFirst(TreeScope.Descendants, this.ButtonCondition(vDownloadDialogSaveButton));

                        // Establecer el foco al Button de "Save" y hacer {{Enter}}
                        vDownloadDialogSaveButtonElement.SetFocus();
                        SendKeys.SendWait(ENTER_KEY);
                        #endregion

                        #region SaveAsDialog
                        // Obtener el Dialog de "File SaveAs" por medio del título y el nombre de la ventana que lo levantó
                        AutomationElement vSaveAsDialogWindow = vDesktopObject.FindFirst(TreeScope.Children, this.NameProperty(vSaveAsDialogTitle));

                        // Obtener el Field de "File name" utilizando ControlId, para esto se usa el Spy++ (Visual Studio 2013: Menú > Tools > Spy++)
                        AutomationElement vSaveAsDialogFileNameFieldElement = vSaveAsDialogWindow.FindFirst(TreeScope.Descendants, this.FieldIdCondition(vSaveAsDialogFileNameFieldId));
                        ValuePattern vSaveAsDialogFileNameFieldValuePattern = vSaveAsDialogFileNameFieldElement.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;

                        // Establecer la dirección y nombre del archivos con extensión que se va a descargar
                        vSaveAsDialogFileNameFieldValuePattern.SetValue(DEFAULT_ATTACHMENTS_FOLDER + vFileNameToDownload);

                        // Obtener el Button de "Save"
                        AutomationElement vSaveAsDialogSaveButtonElement = vSaveAsDialogWindow.FindFirst(TreeScope.Descendants, this.ButtonCondition(vSaveAsDialogSaveButton));

                        // Establecer el foco al Button de "Save" y hacer {{Enter}}
                        vSaveAsDialogSaveButtonElement.SetFocus();
                        SendKeys.SendWait(ENTER_KEY);
                        #endregion
                    }
                    catch (Exception vE)
                    {
                        logger.Error(vE);
                    }
                });

                // Hacer click al enlace para descargar el archivo
                HtmlHelper.ClickElement(this.webBrowser.Document, vIdentifierElement, true);
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        private void UploadFile()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(500);
                    try
                    {
                        AutomationElement vDesktopObject = AutomationElement.RootElement;
                        PropertyCondition vWebBrowserName = new PropertyCondition(AutomationElement.NameProperty, WEBBROWSER_TITLE);
                        AutomationElement vWebBrowserWindow = vDesktopObject.FindFirst(TreeScope.Children, vWebBrowserName);
                        PropertyCondition vUploadDialogName = new PropertyCondition(AutomationElement.NameProperty, "Choose File to Upload");
                        AutomationElement vUploadDialogWindow = vWebBrowserWindow.FindFirst(TreeScope.Children, vUploadDialogName);
                        AndCondition vUploadDialogFileNameFieldCondition = new AndCondition(
                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
                            new PropertyCondition(AutomationElement.AutomationIdProperty, "1148")
                        );
                        AutomationElement vUploadDialogFileNameFieldElement = vUploadDialogWindow.FindFirst(TreeScope.Descendants, vUploadDialogFileNameFieldCondition);
                        ValuePattern vUploadDialogFileNameFieldValuePattern = vUploadDialogFileNameFieldElement.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                        vUploadDialogFileNameFieldValuePattern.SetValue(DEFAULT_ATTACHMENTS_FOLDER + "report_108370888.pdf");
                        AndCondition vUploadDialogOpenButtonCondition = new AndCondition(
                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                            new PropertyCondition(AutomationElement.NameProperty, CANCEL_BUTTON)
                        );
                        AutomationElement vUploadDialogOpenButtonElement = vWebBrowserWindow.FindFirst(TreeScope.Descendants, vUploadDialogOpenButtonCondition);
                        vUploadDialogOpenButtonElement.SetFocus();
                        SendKeys.SendWait(ENTER_KEY);
                    }
                    catch (Exception vE)
                    {
                        throw vE;
                    }
                });
                HtmlHelper.ClickElement(this.webBrowser.Document, "UCCargarDocAutorizacion1_flDocumento", true);
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        private PropertyCondition NameProperty(string pName)
        {
            return new PropertyCondition(AutomationElement.NameProperty, pName);
        }

        private AndCondition ButtonCondition(string pButton)
        {
            return new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                this.NameProperty(pButton)
            );
        }

        private AndCondition FieldIdCondition(string pFieldId)
        {
            return new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
                new PropertyCondition(AutomationElement.AutomationIdProperty, pFieldId)
            );
        }
    }
}