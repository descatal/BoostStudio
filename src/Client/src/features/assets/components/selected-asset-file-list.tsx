import React from "react";
import { AssetFileVm } from "@/api/exvs";
import { Scroller } from "@/components/ui/scroller";
import { Badge } from "@/components/ui/badge";
import { HashInput } from "@/components/custom/hash-input";
import { useVirtualizer } from "@tanstack/react-virtual";

interface SelectedAssetFileListProps {
  value: AssetFileVm[] | undefined;
}

const SelectedAssetFileList = ({ value }: SelectedAssetFileListProps) => {
  const parentRef = React.useRef(null);
  const rowVirtualizer = useVirtualizer({
    count: value?.length ?? 0,
    getScrollElement: () => parentRef.current,
    estimateSize: () => 100,
  });

  return (
    <div>
      {value ? (
        <Scroller
          className={"flex max-h-72 overflow-auto flex-col p-4"}
          ref={parentRef}
          hideScrollbar
        >
          <div
            style={{
              height: `${rowVirtualizer.getTotalSize()}px`,
              width: "100%",
              position: "relative",
            }}
          >
            {rowVirtualizer.getVirtualItems().map((virtualRow) => (
              <div
                key={virtualRow.index}
                className={"flex flex-col gap-2"}
                style={{
                  position: "absolute",
                  top: 0,
                  left: 0,
                  width: "100%",
                  height: `${value[virtualRow.index]}px`,
                  transform: `translateY(${virtualRow.start}px)`,
                }}
              >
                {value[virtualRow.index] && (
                  <>
                    <div className={"flex gap-2"}>
                      {value[virtualRow.index].unitIds &&
                        value[virtualRow.index].unitIds!.length > 0 && (
                          <Badge
                            className={"h-fit justify-center"}
                            variant={"default"}
                          >
                            {value[virtualRow.index].unitIds}
                          </Badge>
                        )}
                      <Badge
                        className={"h-fit justify-center"}
                        variant={"outline"}
                      >
                        {value[virtualRow.index].fileType}
                      </Badge>
                    </div>
                    <HashInput
                      readonly={true}
                      initialMode={"hex"}
                      initialValue={value[virtualRow.index].hash}
                      placeholder={"e.g. 1A42E312"}
                    />
                  </>
                )}
              </div>
            ))}
          </div>
        </Scroller>
      ) : (
        <div>
          <p>No Files Selected.</p>
        </div>
      )}
    </div>
  );
};

export default SelectedAssetFileList;
