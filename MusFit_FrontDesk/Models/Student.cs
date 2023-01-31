using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MusFit_FrontDesk.Models
{
    public partial class Student
    {
        public Student()
        {
            ClassOrders = new HashSet<ClassOrder>();
            ClassRecords = new HashSet<ClassRecord>();
            InBodies = new HashSet<InBody>();
        }

        public int SId { get; set; }
        
        public string SNumber { get; set; }

        [Display(Name = "真實姓名")]
        [Required(ErrorMessage = "您必須填入真實姓名")]
        [StringLength(5, ErrorMessage = "最多輸入五個中文字")]
        public string SName { get; set; }

        [Required(ErrorMessage = "您必須填入電子信箱")]
        [Display(Name = "電子信箱")]
        [EmailAddress(ErrorMessage = "只允許輸入Email格式")]
        public string SMail { get; set; }

        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "您需要選擇您的生日")]
        public DateTime? SBirth { get; set; }

        [Required(ErrorMessage = "您必須選擇性別")]
        [Display(Name = "性別")]
        public bool SGender { get; set; }

        [Required(ErrorMessage = "您必須填入緊急聯絡人")]
        [DisplayName("緊急聯絡人")]
        [StringLength(50)]
        public string SContactor { get; set; }

        [Required(ErrorMessage = "您必須填入緊急聯絡人電話")]
        [DisplayName("緊急聯絡人電話")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^09[0-9]{8}$",ErrorMessage = "請輸入正確手機號碼格式!")]
        public string SContactPhone { get; set; }
        public string SPhoto { get; set; }
        public string SAddress { get; set; }

        [Required(ErrorMessage = "您必須填入手機號碼")]
        [DisplayName("手機號碼")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "請輸入正確手機號碼格式!")]
        public string SPhone { get; set; }
        public string SAccount { get; set; }
        public byte[] SPassword { get; set; }
        public string SToken { get; set; }
        public DateTime? SJoinDate { get; set; }
        public bool? SIsStudentOrNot { get; set; }

        public virtual ICollection<ClassOrder> ClassOrders { get; set; }
        public virtual ICollection<ClassRecord> ClassRecords { get; set; }
        public virtual ICollection<InBody> InBodies { get; set; }
    }
}
