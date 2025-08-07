using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs
{
    public class ResponseDto<TDto>where TDto : class
    {
        //Eğerki Status Code başarılı dönerse ben veriyide çağırabilirim
        public TDto Data { get; private set; }
        public int StatusCode { get;private set; }
        public string Error { get;private set; }
        [JsonIgnore]//JsonData serialize edilirken IsSuccessful propum yok sayılsın
        public bool IsSuccessful { get;private set; }//bidaha uğraşmayalım başka Apilerde responseDto sınıfımı çağırdığımda IsSuccessful propumdan öğreneyim başarılı mı değil mi!

        //Başarılı Olduğu durumda success metodunu döncem
        public static ResponseDto<TDto> Success(TDto data,int statusCode)
        {
            return new ResponseDto<TDto>()
            {
                Data=data,
                StatusCode=statusCode,
                IsSuccessful=true
            };
        }
        //Bazı başarılı işlemlerde data istemeyebilirim
        public static ResponseDto<TDto> Success(int statusCode)
        {
            return new ResponseDto<TDto>()
            {
                Data = default,
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }
        //Hata Aldığımızda dönecek metodum
        public static ResponseDto<TDto> Fail(ErrorDto errorDto,int statusCode)
        {
            return new ResponseDto<TDto>
            {
                StatusCode = statusCode,
                Error = errorDto.ToString(),
                IsSuccessful = false
            };
        }
        public static ResponseDto<TDto> Fail(string errorMessage,int statusCode,bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage.ToString(), isShow);
            return new ResponseDto<TDto>()
            {
                Error = errorDto.ToString(),
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }
    }
}
