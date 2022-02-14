using System;

namespace EWP
{
    class SboMessage
    {
        public static void Send(string subject, SboException exception)
        {
            SAPbobsCOM.MessagesService cmpnySrvc = null;
            SAPbobsCOM.Message message = null;

            try
            {
                string text = string.Format("{0} [{1}]\n{2}", exception.ErrorMessage, exception.ErrorCode, exception.StackTrace);

                cmpnySrvc = (SAPbobsCOM.MessagesService)SboClient.Instance.Company.GetCompanyService().GetBusinessService(SAPbobsCOM.ServiceTypes.MessagesService);
                message = (SAPbobsCOM.Message)cmpnySrvc.GetDataInterface(SAPbobsCOM.MessagesServiceDataInterfaces.msdiMessage);

                message.Subject = subject;
                message.Text = text;
                message.Priority = SAPbobsCOM.BoMsgPriorities.pr_High;

                SAPbobsCOM.RecipientCollection recipientCollection = message.RecipientCollection;
                SAPbobsCOM.Recipient recipient = recipientCollection.Add();
                recipient.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;
                recipient.UserCode = SboClient.Instance.Company.UserName;

                cmpnySrvc.SendMessage(message);
            }
            catch (Exception ex)
            {

                SboStatusBar.SetTextAsError("An occurred has been ocurred to send an SAP alert. " + ex.Message);
            }
            finally
            {
                SboMarshal.ReleaseMemory();
                SboMarshal.ReleaseComObject(cmpnySrvc);
                SboMarshal.ReleaseComObject(message);
            }
        }

        public static void Send(string subject, Exception exception)
        {
            SAPbobsCOM.MessagesService cmpnySrvc = null;
            SAPbobsCOM.Message message = null;

            try
            {
                string text = string.Format("{0}\n{1}", exception.Message, exception.StackTrace);

                cmpnySrvc = (SAPbobsCOM.MessagesService)SboClient.Instance.Company.GetCompanyService().GetBusinessService(SAPbobsCOM.ServiceTypes.MessagesService);
                message = (SAPbobsCOM.Message)cmpnySrvc.GetDataInterface(SAPbobsCOM.MessagesServiceDataInterfaces.msdiMessage);

                message.Subject = subject;
                message.Text = text;
                message.Priority = SAPbobsCOM.BoMsgPriorities.pr_High;

                SAPbobsCOM.RecipientCollection recipientCollection = message.RecipientCollection;
                SAPbobsCOM.Recipient recipient = recipientCollection.Add();
                recipient.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;
                recipient.UserCode = SboClient.Instance.Company.UserName;

                cmpnySrvc.SendMessage(message);
            }
            catch (Exception ex)
            {

                SboStatusBar.SetTextAsError("An occurred has been ocurred to send an SAP alert. " + ex.Message);
            }
            finally
            {
                SboMarshal.ReleaseMemory();
                SboMarshal.ReleaseComObject(cmpnySrvc);
                SboMarshal.ReleaseComObject(message);
            }
        }
    }
}
