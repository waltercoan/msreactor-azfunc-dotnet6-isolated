using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace br.com.waltercoan.azfuncisolated;
public class FuncGeoDataImgBlobTrigger
{
    private FuncDbContext dbContext;
    public FuncGeoDataImgBlobTrigger(FuncDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    [Function("FuncGeoDataImgBlobTrigger")]
    public async Task RunAsync([BlobTrigger("samples-workitems/{name}", Connection = "STAZFUNC6_STORAGE")] byte[] myBlob, string name,
        FunctionContext context)
    {
        var logger = context.GetLogger("FuncGeoDataImgBlobTrigger");
        MemoryStream imageStream = new MemoryStream(myBlob);
        try
        {    
            var photoItem = new PhotoItem(){
                RowKey = Guid.NewGuid().ToString(),
                Path = context.BindingContext.BindingData["Uri"].ToString()
            };
            try
            {
                var gps = ImageMetadataReader.ReadMetadata(imageStream)
                                .OfType<GpsDirectory>()
                                .FirstOrDefault();

                var location = gps.GetGeoLocation();

                photoItem.Latitude = location.Latitude;
                photoItem.Longitude = location.Longitude;
            }
            catch (System.Exception e)
            {
                logger.LogWarning(e, e.Message);
            }


            dbContext.PhotoItens.Add(photoItem);
            await dbContext.SaveChangesAsync();

            logger.LogTrace($"{photoItem.Path} at {photoItem.Latitude},{photoItem.Longitude}");
        }
        catch (System.Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}

