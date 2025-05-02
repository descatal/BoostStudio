import React from "react";

interface AssetsFileSearcherProps {
  value?: number | undefined;
  defaultValue?: number | undefined;
  defaultUnitId?: number | undefined;
}

const AssetsFileSearcher = ({
  value,
  defaultValue,
  defaultUnitId,
}: AssetsFileSearcherProps) => {
  const [selectedTab, setSelectedTab] = React.useState<"unit" | "common">(
    "unit",
  );

  return <></>;
};

export default AssetsFileSearcher;
