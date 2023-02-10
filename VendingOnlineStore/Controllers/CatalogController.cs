using System.ComponentModel.DataAnnotations;
using Core.Services.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models.Catalog;

namespace VendingOnlineStore.Controllers;

[Authorize]
public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] string city = "Perm")
    {
        var options = _catalogService.GetOptions(city);

        var model = options.Select(MapToOptionViewModel);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToBagAsync([FromForm, Required] NewBagContentViewModel form)
    {
        var newBagContent = MapToNewBagContent(form);
        var bagContent = await _catalogService.AddToBagAsync(newBagContent);

        return BagContentPartialView(bagContent, form.Index, form.OptionIndex);
    }

    [HttpPost]
    public async Task<IActionResult> IncreaseContentCountAsync([FromForm, Required] BagContentIdViewModel form)
    {
        var bagContent = await _catalogService.IncreaseContentCountAsync(form.Id);

        return BagContentPartialView(bagContent, form.Index, form.OptionIndex);
    }

    [HttpPost]
    public async Task<IActionResult> DecreaseContentCountAsync([FromForm, Required] BagContentIdViewModel form)
    {
        var bagContent = await _catalogService.DecreaseContentCountAsync(form.Id);

        return BagContentPartialView(bagContent, form.Index, form.OptionIndex);
    }

    private static OptionViewModel MapToOptionViewModel(OptionDetails optionDetails, int optionIndex)
    {
        var item = MapToItemViewModel(optionDetails.Item);
        var contentCollection = MapToContentCollection(optionDetails.Contents, optionIndex);
        var optionViewModel = new OptionViewModel(
            item,
            contentCollection
        );
        return optionViewModel;
    }

    private static ContentCollectionViewModel MapToContentCollection(IEnumerable<ContentDetails> optionContents,
        int optionIndex)
    {
        var contents = optionContents
            .Select((content, index) => MapToContentViewModel(content, index, optionIndex));
        var contentCollection = new ContentCollectionViewModel(
            optionIndex,
            contents
        );
        return contentCollection;
    }

    private static ItemViewModel MapToItemViewModel(ItemDetails itemDetails)
    {
        var itemViewModel = new ItemViewModel(
            itemDetails.Name,
            itemDetails.Description,
            itemDetails.PhotoLink
        );
        return itemViewModel;
    }

    private static ContentViewModel MapToContentViewModel(ContentDetails contentDetails, int index, int optionIndex)
    {
        var pickupPoint = MapToPickupPointViewModel(contentDetails.PickupPoint);
        var bagContent = MapToBagContentViewModel(contentDetails.BagContent, index, optionIndex);
        var contentViewModel = new ContentViewModel(
            optionIndex,
            pickupPoint,
            contentDetails.Price,
            bagContent
        );
        return contentViewModel;
    }

    private static PickupPointViewModel MapToPickupPointViewModel(PickupPointDetails pickupPointDetails)
    {
        var pickupPointViewModel = new PickupPointViewModel(
            pickupPointDetails.Address,
            pickupPointDetails.Description
        );
        return pickupPointViewModel;
    }

    private static BagContentViewModel MapToBagContentViewModel(BagContent bagContent, int index, int optionIndex)
    {
        var bagEntrance = MapToBagEntranceViewModel(bagContent.BagEntrance);
        var bagContentViewModel = new BagContentViewModel(
            index,
            optionIndex,
            bagContent.ItemId,
            bagContent.PickupPointId,
            bagEntrance
        );
        return bagContentViewModel;
    }

    private static BagEntranceViewModel? MapToBagEntranceViewModel(BagEntrance? bagEntrance)
    {
        if (bagEntrance == default)
            return default;

        var bagEntranceViewModel = new BagEntranceViewModel(
            bagEntrance.BagContentId,
            bagEntrance.Count
        );
        return bagEntranceViewModel;
    }

    private static NewBagContent MapToNewBagContent(NewBagContentViewModel newBagContentViewModel)
    {
        var newBagContent = new NewBagContent(
            newBagContentViewModel.ItemId,
            newBagContentViewModel.PickupPointId
        );
        return newBagContent;
    }

    private PartialViewResult BagContentPartialView(BagContent bagContent, int index, int optionIndex)
    {
        var bagContentModel = MapToBagContentViewModel(bagContent, index, optionIndex);
        return PartialView("_BagContent", bagContentModel);
    }
}