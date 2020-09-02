using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPMVC.Models
{
    public class CONSOLIDATED_LISTM
    {
        [Key]
       public int Id { get; set; }
        public List<INDIVIDUALM> iNDIVIDUALS { get; set; }

        public List<ENTITYM> ENTITIES { get; set; }

        public System.DateTime dateGenerated { get; set; }
    }



    public partial class INDIVIDUALM
    {
        [Key]
        public int Id { get; set; }

        public string DATAID { get; set; }

        public string VERSIONNUM { get; set; }

        public string FIRST_NAME { get; set; }

        public string SECOND_NAME { get; set; }

        public string THIRD_NAME { get; set; }

        public string FOURTH_NAME { get; set; }

        public string UN_LIST_TYPE { get; set; }

        public string REFERENCE_NUMBER { get; set; }

        public System.DateTime LISTED_ON { get; set; }

        public string GENDER { get; set; }

        public string SUBMITTED_BY { get; set; }

        public string NAME_ORIGINAL_SCRIPT { get; set; }

        public string COMMENTS1 { get; set; }

        public string NATIONALITY2 { get; set; }

        public string TITLE { get; set; }

        public string DESIGNATION { get; set; }

        public string NATIONALITY { get; set; }

        public LIST_TYPEM LIST_TYPE { get; set; }

        public string LAST_DAY_UPDATED { get; set; }

        public List<INDIVIDUAL_ALIASM> INDIVIDUAL_ALIAS { get; set; }

        public List<INDIVIDUAL_ADDRESSM> INDIVIDUAL_ADDRESS { get; set; }

        public List<INDIVIDUAL_DATE_OF_BIRTHM> INDIVIDUAL_DATE_OF_BIRTH { get; set; }

        public List<INDIVIDUAL_PLACE_OF_BIRTHM> INDIVIDUAL_PLACE_OF_BIRTH { get; set; }

        public List<INDIVIDUAL_DOCUMENTM> INDIVIDUAL_DOCUMENT { get; set; }

        public string SORT_KEY { get; set; }

        public string SORT_KEY_LAST_MOD { get; set; }

        public System.DateTime DELISTED_ON { get; set; }

        public bool DELISTED_ONSpecified { get; set; }
    }



    public partial class LIST_TYPEM
    {
        [Key]
         public int Id { get; set; }

        public string VALUE { get; set; }
    }


    public partial class INDIVIDUAL_ALIASM
    {
        [Key]
        public int Id { get; set; }
        public string QUALITY { get; set; }

        public string ALIAS_NAME { get; set; }

        public string DATE_OF_BIRTH { get; set; }

        public string CITY_OF_BIRTH { get; set; }

        public string COUNTRY_OF_BIRTH { get; set; }

        public string NOTE { get; set; }
    }

    public partial class INDIVIDUAL_ADDRESSM
    {
        [Key]
         public int Id { get; set; }

        public string STREET { get; set; }

        public string CITY { get; set; }

        public string STATE_PROVINCE { get; set; }

        public string ZIP_CODE { get; set; }

        public string COUNTRY { get; set; }

        public string NOTE { get; set; }
    }


    public partial class INDIVIDUAL_DATE_OF_BIRTHM
    {
        [Key]
        public int Id { get; set; }
        public string TYPE_OF_DATE { get; set; }

        public string NOTE { get; set; }

        public string Items { get; set; }

        public string ItemsElementName { get; set; }
    }

    public partial class INDIVIDUAL_PLACE_OF_BIRTHM
    {
        [Key]
         public int Id { get; set; }

        public string CITY { get; set; }

        public string STATE_PROVINCE { get; set; }

        public string COUNTRY { get; set; }

        public string NOTE { get; set; }
    }

        public partial class INDIVIDUAL_DOCUMENTM
    {
        [Key]
        public int Id { get; set; }

        public string TYPE_OF_DOCUMENT { get; set; }

        public string TYPE_OF_DOCUMENT2 { get; set; }

        public string NUMBER { get; set; }

        public string ISSUING_COUNTRY { get; set; }

        public string DATE_OF_ISSUE { get; set; }

        public string CITY_OF_ISSUE { get; set; }

        public string COUNTRY_OF_ISSUE { get; set; }

        public string NOTE { get; set; }

    }

    public partial class ENTITYM
    {
        [Key]
      public int Id { get; set; }

        public string DATAID { get; set; }

        public string VERSIONNUM { get; set; }

        public string FIRST_NAME { get; set; }

        public string UN_LIST_TYPE { get; set; }

        public string REFERENCE_NUMBER { get; set; }

        public System.DateTime LISTED_ON { get; set; }

        public System.DateTime SUBMITTED_ON { get; set; }

        public bool SUBMITTED_ONSpecified { get; set; }

        public string NAME_ORIGINAL_SCRIPT { get; set; }

        public string COMMENTS1 { get; set; }

        public LIST_TYPEM LIST_TYPE { get; set; }

        public string LAST_DAY_UPDATED { get; set; }

        public List<ENTITY_ALIASM> ENTITY_ALIAS { get; set; }

        public List<ENTITY_ADDRESSM> ENTITY_ADDRESS { get; set; }

        public string SORT_KEY { get; set; }

        public string SORT_KEY_LAST_MOD { get; set; }

        public System.DateTime DELISTED_ON { get; set; }

        public bool DELISTED_ONSpecified { get; set; }

    }



    public partial class ENTITY_ALIASM
    {
        [Key]
         public int Id { get; set; }

        public string QUALITY { get; set; }

        public string ALIAS_NAME { get; set; }
    }

    public partial class ENTITY_ADDRESSM
    {
        [Key]
       public int Id { get; set; }

        public string STREET { get; set; }

        public string CITY { get; set; }

        public string STATE_PROVINCE { get; set; }

        public string ZIP_CODE { get; set; }

        public string COUNTRY { get; set; }

        public string NOTE { get; set; }
    }


    public partial class INDIVIDUALSM
    {
        [Key]
        public int Id { get; set; }

        public List<INDIVIDUALM> INDIVIDUAL { get; set; }
    }

    public partial class TITLEM
    {
        [Key]
       public int Id { get; set; }

        public string VALUE { get; set; }
    }


    public partial class DESIGNATIONM
    {
        [Key]
       public int Id { get; set; }

        public string VALUE { get; set; }
    }
    public partial class NATIONALITYM
    {
        [Key]
      public int Id { get; set; }

        public string VALUE { get; set; }
    }


    public partial class ENTITIESM
    {
       
        [Key]
       public int Id { get; set; }

        public List<ENTITYM> ENTITY { get; set; }

    }

    public partial class LAST_DAY_UPDATEDM
    {
        [Key]
        public int Id { get; set; }
        public string VALUE { get; set; }

    }



    }
