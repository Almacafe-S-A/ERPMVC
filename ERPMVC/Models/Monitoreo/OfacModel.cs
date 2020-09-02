
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OFAC
{
    public class sdnListM
    {
        [Key]
        public int Id { get; set; }

        public int sdnListPublshInformationId { get; set; }
        public sdnListPublshInformationM publshInformation { get; set; }
        public List<sdnListSdnEntryM> sdnEntry { get; set; }
    }

    public class sdnListPublshInformationM
    {
        [Key]
        public int Id { get; set; }
        public string Publish_Date { get; set; }
        public int Record_Count { get; set; }
        public bool Record_CountSpecified { get; set; }
    }

    public class sdnListSdnEntryM
    {
        [Key]
        public int Id { get; set; }

        public int uid { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string title { get; set; }
        public string sdnType { get; set; }
        public string remarks { get; set; }
        public string programList { get; set; }
        public List<sdnListSdnEntryIDM> idList { get; set; }
        public List<sdnListSdnEntryAkaM> akaList { get; set; }
        public List<sdnListSdnEntryAddressM> addressList { get; set; }
        public List<sdnListSdnEntryNationalityM> nationalityList { get; set; }
        public List<sdnListSdnEntryCitizenshipM> citizenshipList { get; set; }
        public List<sdnListSdnEntryDateOfBirthItemM> dateOfBirthList { get; set; }
        public List<sdnListSdnEntryPlaceOfBirthItemM> placeOfBirthList { get; set; }
        public sdnListSdnEntryVesselInfoM vesselInfo { get; set; }
    }

    public class sdnListSdnEntryIDM
    {
        [Key]
        public int Id { get; set; }
        public int sdnListSdnEntryMId { get; set; }

        public int uid { get; set; }
        public string idType { get; set; }
        public string idNumber { get; set; }
        public string idCountry { get; set; }
        public string issueDate { get; set; }
        public string expirationDate { get; set; }
    }

    public class sdnListSdnEntryAkaM
    {
        [Key]
        public int Id { get; set; }
        public int sdnListSdnEntryMId { get; set; }
 
        public int uid { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
    }

    public class sdnListSdnEntryAddressM
    {
        [Key]
        public int Id { get; set; }
        public int sdnListSdnEntryMId { get; set; }
        
        public int uid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public string stateOrProvince { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
    }

    public class sdnListSdnEntryNationalityM
    {
        [Key]
        public int Id { get; set; }
        public int sdnListSdnEntryMId { get; set; }
        
        public int uid { get; set; }
        public string country { get; set; }
        public bool mainEntry { get; set; }
    }

    public class sdnListSdnEntryCitizenshipM
    {
        [Key]
        public int Id { get; set; }
        public int sdnListSdnEntryMId { get; set; }
        
        public int uid { get; set; }
        public string country { get; set; }
        public bool mainEntry { get; set; }
    }

    public class sdnListSdnEntryDateOfBirthItemM
    {
        [Key]
        public int Id { get; set; }
        public int sdnListSdnEntryMId { get; set; }
 
        public int uid { get; set; }
        public string dateOfBirth { get; set; }
        public bool mainEntry { get; set; }
    }

    public class sdnListSdnEntryPlaceOfBirthItemM
    {
        [Key]
        public int Id { get; set; }
        public int sdnListSdnEntryMId { get; set; }
 
        public int uid { get; set; }
        public string placeOfBirth { get; set; }
        public bool mainEntry { get; set; }
    }

    public class sdnListSdnEntryVesselInfoM
    {
        [Key]
        public int Id { get; set; }

        public string callSign { get; set; }
        public string vesselType { get; set; }
        public string vesselFlag { get; set; }
        public string vesselOwner { get; set; }
        public int tonnage { get; set; }
        public bool tonnageSpecified { get; set; }
        public int grossRegisteredTonnage { get; set; }
        public bool grossRegisteredTonnageSpecified { get; set; }
    }

}
