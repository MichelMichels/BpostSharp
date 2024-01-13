using BpostSharp.Models;
using BpostSharp.Services;
using NPOI.SS.UserModel;

namespace BpostSharp.Excel;

public class ExcelCacheBuilder(string sourceFilePath) : ICacheBuilder<CityData>
{
    private bool isBuilding;

    private readonly List<CityData> cache = [];

    private readonly string sourceFilePath = sourceFilePath ?? throw new ArgumentNullException(nameof(sourceFilePath));

    public Task Clear()
    {
        cache.Clear();
        return Task.CompletedTask;
    }

    public async Task Build()
    {
        if (isBuilding)
        {
            return;
        }

        isBuilding = true;

        await Clear();

        ISheet sheet;

        using FileStream stream = new(sourceFilePath, FileMode.Open);
        stream.Position = 0;
        IWorkbook workbook = WorkbookFactory.Create(stream);
        sheet = workbook.GetSheetAt(0);
        IRow headerRow = sheet.GetRow(0);
        int cellCount = headerRow.LastCellNum;

        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null)
            {
                continue;
            }

            if (row.Cells.All(d => d.CellType == CellType.Blank))
            {
                continue;
            }

            CityData data = ConvertRow(row);

            cache.Add(data);
        }

        isBuilding = false;
    }
    public IEnumerable<CityData> Get() => cache;
    public bool HasCache() => cache.Count != 0;

    private CityData ConvertRow(IRow row)
    {
        ICell? postalCodeCell = row.GetCell(0);
        ICell? cityNameCell = row.GetCell(1);
        ICell? isMunicipalityCell = row.GetCell(2);
        ICell? mainMunicipalityCell = row.GetCell(3);
        ICell? provinceCell = row.GetCell(4);

        string postalCode = postalCodeCell?.ToString() ?? string.Empty;
        string cityName = cityNameCell?.ToString() ?? string.Empty;
        string mainMunicipality = mainMunicipalityCell?.ToString() ?? string.Empty;
        string province = provinceCell?.ToString() ?? string.Empty;

        CityData data = new()
        {
            PostalCode = postalCode,
            Name = cityName,
            MainMunicipality = mainMunicipality,
            Province = province,
        };

        if (isMunicipalityCell != null)
        {
            data.IsMunicipality = (isMunicipalityCell.ToString()) switch
            {
                "Ja" => true,
                "Oui" => true,
                "Neen" => false,
                "Non" => false,
                "" => null,
                _ => null,
            };
        }
        else
        {
            data.IsMunicipality = null;
        }

        return data;
    }
}
