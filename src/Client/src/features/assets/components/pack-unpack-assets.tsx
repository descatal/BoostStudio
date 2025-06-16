import { useMediaQuery } from "@/hooks/use-media-query";
import PackUnpackAssetCard from "@/features/assets/components/cards/pack-unpack-asset-card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";

interface Props {
  unitIds?: number[];
}

const PackUnpackAssets = ({ unitIds }: Props) => {
  const isDesktop = useMediaQuery("(min-width: 800px)");

  if (isDesktop) {
    return (
      <div
        className={
          "h-full grid grid-cols-2 gap-4 justify-items-center items-center"
        }
      >
        <PackUnpackAssetCard
          className={"max-w-[640px]"}
          type={"Pack"}
          unitIds={unitIds}
        />
        <PackUnpackAssetCard
          className={"max-w-[640px]"}
          type={"Unpack"}
          unitIds={unitIds}
        />
      </div>
    );
  }

  return (
    <Tabs defaultValue={"pack"}>
      <TabsList>
        <TabsTrigger value="pack">Pack</TabsTrigger>
        <TabsTrigger value="unpack">Unpack</TabsTrigger>
      </TabsList>
      <TabsContent value={"pack"}>
        <PackUnpackAssetCard type={"Pack"} unitIds={unitIds} />
      </TabsContent>
      <TabsContent value={"unpack"}>
        <PackUnpackAssetCard type={"Unpack"} unitIds={unitIds} />
      </TabsContent>
    </Tabs>
  );
};

export default PackUnpackAssets;
