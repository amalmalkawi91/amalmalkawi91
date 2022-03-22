using System;
using System.Collections.Generic;
using System.Text;
using System;
using Newtonsoft.Json;

namespace Aquary.Models
{
    public class Repository
    {
        #region Common
        [JsonProperty("main_service_id")]
        public int main_service_id { get; set; }

        [JsonProperty("sub_service_id")]
        public int sub_service_id { get; set; }

        [JsonProperty("FK_Sub_Service")]
        public int FK_Sub_Service { get; set; }

        [JsonProperty("service_ar")]
        public string service_ar { get; set; }

        [JsonProperty("service_en")]
        public string service_en { get; set; }

        [JsonProperty("image")]
        public string image { get; set; }

        [JsonProperty("logo")]
        public string logo { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("description_en")]
        public string description_en { get; set; }

        [JsonProperty("ads_id")]
        public string ads_id { get; set; }

        
        [JsonProperty("code")]
        public string code { get; set; }


        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("comment_time")]
        public string comment_time { get; set; }

        [JsonProperty("register_name")]
        public string register_name { get; set; }

        [JsonProperty("name_en")]
        public string name_en { get; set; }

        [JsonProperty("name_ar")]
        public string name_ar { get; set; }

        [JsonProperty("city_id")]
        public string city_id { get; set; }

        [JsonProperty("region_id")]
        public string region_id { get; set; }
        [JsonProperty("result")]
        public string result { get; set; }
        [JsonProperty("msg_en")]
        public string msg_en { get; set; }
        [JsonProperty("msg_ar")]
        public string msg_ar { get; set; }
        #endregion
        [JsonProperty("price")]
        public string price { get; set; }
        [JsonProperty("Default_Price")]
        public string Default_Price { get; set; }

        [JsonProperty("organizer_name_en")]
        public string organizer_name_en { get; set; }

        [JsonProperty("organizer_name_ar")]
        public string organizer_name_ar { get; set; }

        [JsonProperty("organizer_id")]
        public string organizer_id { get; set; }

        [JsonProperty("region_ar")]
        public string region_ar { get; set; }

        [JsonProperty("region_en")]
        public string region_en { get; set; }

        [JsonProperty("area")]
        public string area { get; set; }

        [JsonProperty("area_ar")]
        public string area_ar { get; set; }

        [JsonProperty("land_area_value")]
        public int land_area_value { get; set; }

        [JsonProperty("building_area")]
        public int building_area { get; set; }

        [JsonProperty("number_room")]
        public int number_room { get; set; }

        [JsonProperty("number_bath")]
        public int number_bath { get; set; }

        [JsonProperty("ads_visit_count")]
        public int ads_visit_count { get; set; }

        [JsonProperty("Admin_Notification_Id")]
        public int Admin_Notification_Id { get; set; }

        [JsonProperty("Text_En")]
        public string Text_En { get; set; }



        [JsonProperty("Text_Ar")]
        public string Text_Ar { get; set; }


        [JsonProperty("FK_Ads_Id")]
        public string FK_Ads_Id { get; set; }

      


       // [JsonProperty("code")]
      //  public string code { get; set; }

        #region Ads_Deco_Form



        // [JsonProperty("has_ads")]
        // public int has_ads { get; set; }

        public string comp_name { get; set; }
        public int fk_main_service { get; set; }
        public int fk_sub_service { get; set; }
        public int fk_city_id { get; set; }
        public int fk_region_id { get; set; }
        public int FK_Register_Id { get; set; }
        public int fk_ads_pack_id { get; set; }
        public int fk_AdvertiserType_id { get; set; }
        public string facebook_link { get; set; }
        public string instagram_link { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string comp_description { get; set; }

        public string register_logo { get; set; }

        public Boolean has_ads{ get; set; }
        
        public int ads_status { get; set; }
        public int allow_comments { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        public string latitude { get; set; }
        public string longitude { get; set; }
        #endregion

        #region Save_Photo
      
        public string Image_Path { get; set; }
        #endregion

    }
}
