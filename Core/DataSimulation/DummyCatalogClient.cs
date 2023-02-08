using Core.Clients.Catalog;
using static Core.DataSimulation.Data;

namespace Core.DataSimulation;

public class DummyCatalogClient : ICatalogClient
{
    public IReadOnlyCollection<Option> GetOptions(string city)
    {
        var r = new List<Option>();

        foreach (var item in Items)
        {
            var contents = new List<Content>();
            foreach (var pickupPointContent in PickupPointsContents)
            {
                if (pickupPointContent.Item != item)
                    continue;

                var p = pickupPointContent.PickupPoint;
                var pickupPoint = new PickupPoint(p.Id, p.Address, p.Description);
                var price = pickupPointContent.Price;
                var c = new Content(pickupPoint, price);
                contents.Add(c);
            }

            if (!contents.Any())
                continue;

            var i = new Item(item.Id, item.Name, item.Description, item.PhotoLink);
            var o = new Option(i, contents);
            r.Add(o);
        }

        return r;
    }

    public IReadOnlyCollection<Option> GetOptions(Place place)
    {
        throw new NotImplementedException();
    }
}