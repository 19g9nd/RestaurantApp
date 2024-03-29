using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RestaurauntApp.Models.Other;
using RestaurauntApp.Services;

namespace RestaurauntApp.Models
{
    public class DiscountCode
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The code is required.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "The discount percent is required.")]
        [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100.")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "The valid from date is required.")]
        [Display(Name = "Valid From")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidFrom { get; set; }

        [Required(ErrorMessage = "The valid to date is required.")]
        [Display(Name = "Valid To")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FutureDate(ErrorMessage = "The date must be in the future.")]
        [DateShouldBeValid(ErrorMessage = "Valid To date must be greater than or equal to Valid From date.")]
        public DateTime ValidTo { get; set; }

        public virtual ICollection<DiscountUsage> Usages { get; set; }

        public DiscountCode()
        {
            Usages = new List<DiscountUsage>();
            ValidFrom = DateTime.Now;
            ValidTo = DateTime.Today.AddDays(1);
        }
    }

   public class DateShouldBeValidAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime validTo = (DateTime)value;
        var instance = validationContext.ObjectInstance as DiscountCode;
        
        if (instance == null)
        {
            return new ValidationResult("Validation context object is not of type DiscountCode.");
        }

        if (validTo.Date < instance.ValidFrom.Date)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

}
