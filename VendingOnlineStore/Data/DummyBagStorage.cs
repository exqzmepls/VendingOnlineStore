using VendingOnlineStore.Repositories.BagMachine.Dtos;

namespace VendingOnlineStore.Data;

public static class DummyBagStorage
{
    public static readonly IList<BagMachineDto> Content = new List<BagMachineDto>
    {
        new BagMachineDto("1", "m1", new BagItemDto[]
        {
            new BagItemDto("1", "i1", 1),
            new BagItemDto("2", "i2", 5),
            new BagItemDto("3", "i3", 2)
        }),
        new BagMachineDto("2", "m2", new BagItemDto[]
        {
            new BagItemDto("1", "i1", 3),
            new BagItemDto("2", "i2", 1),
            new BagItemDto("3", "i3", 4)
        }),
    };
}
