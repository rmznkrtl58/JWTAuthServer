

using System.Collections.Generic;

namespace SharedLibrary.DTOs
{
    public class ErrorDto
    {
        //private set=> sadece bu classımda set edilebilir
        public List<string> Errors { get; private set; }
        //Eğerki clientim tarafından bir hata varsa göster(True) Clientim tarafında bir hata yoksa developerin görmesi gereken bir hata ise gösterme(False)
        public bool IsShow { get; private set; }

        public ErrorDto()
        {
            Errors = new List<string>();
        }
        public ErrorDto(List<string> errors,bool isShow)
        {
            //birden fazla hata olursa
            Errors = errors;
            IsShow = isShow;
        }
        public ErrorDto(string error,bool isShow)
        {
            //Tek bir hata olursa
            Errors.Add(error);
            IsShow = true;
        }
       

    }
}
