import React from "react";
import { Editor, Monaco } from "@monaco-editor/react";
import OneDarkPro from "@/themes/onedarkpro.json";
import { useMutation, useQuery } from "@tanstack/react-query";
import {
  getApiScexDecompiledByUnitIdOptions,
  postApiScexCompileUnitsMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import { useTheme } from "@/context/theme-context";
import { Option } from "@/components/ui/multiple-selector";
import {
  ResizableHandle,
  ResizablePanel,
  ResizablePanelGroup,
} from "@/components/ui/resizable";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { Icons } from "@/components/icons";
import { SiCompilerexplorer } from "react-icons/si";
import { Separator } from "@/components/ui/separator";
import { Switch } from "@/components/ui/switch";
import { MdMemory } from "react-icons/md";
import UnitsSelector from "@/features/units/components/units-selector";
import { Label } from "recharts";
import { toast } from "sonner";

interface ScriptViewerProps {
  unitId: number;
}

const ScriptCompiler = ({ unitId }: ScriptViewerProps) => {
  const [hotReload, setHotReload] = React.useState(true);
  const [selectedUnitIds, setSelectedUnitIds] = React.useState<Option[]>([]);

  const query = useQuery({
    ...getApiScexDecompiledByUnitIdOptions({
      path: {
        unitId: unitId,
      },
    }),
  });

  const handleEditorDidMount = (monaco: Monaco) => {
    monaco.editor.defineTheme("OneDarkPro", {
      base: "vs-dark",
      inherit: true,
      ...OneDarkPro,
    });
  };

  const { theme } = useTheme();

  const mutation = useMutation({
    ...postApiScexCompileUnitsMutation(),
    onSuccess: (_) => {
      toast("Success", {
        description: `Successfully compiled scripts to working directory!`,
      });
    },
  });

  return (
    <div className={"h-full pb-5 min-h-[350px]"}>
      <ResizablePanelGroup className={"gap-4 mb-6"} direction="horizontal">
        <ResizablePanel
          collapsible={true}
          collapsedSize={0}
          defaultSize={70}
          minSize={30}
        >
          <Editor
            className={"rounded-md border p-2"}
            value={query.data ?? ""}
            theme={theme === "dark" ? "OneDarkPro" : ""}
            beforeMount={handleEditorDidMount}
            defaultLanguage={"C++"}
            options={{
              wordWrap: "on",
              fontSize: 14,
              fontFamily: "Jetbrains-Mono",
              fontLigatures: true,
              readOnly: true,
            }}
          />
        </ResizablePanel>
        <ResizableHandle withHandle />
        <ResizablePanel
          collapsible={true}
          collapsedSize={0}
          defaultSize={30}
          minSize={30}
        >
          <Card className={"h-full"}>
            <CardHeader>
              <CardTitle>Compile</CardTitle>
              <CardDescription>
                Compile units' c format code to .scex compiled binary (005.bin)
                in data folder.
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="grid gap-4">
                <div className={"space-y-2"}>
                  <Label>Units</Label>
                  <UnitsSelector
                    disabled
                    className={"w-full"}
                    defaultValues={[unitId]}
                    values={selectedUnitIds}
                    onChange={setSelectedUnitIds}
                    placeholder={unitId ? undefined : "Select unit..."}
                  />
                </div>
                <div className="flex items-center space-x-4 rounded-md border p-4">
                  <MdMemory size={40} />
                  <div className="flex-1 space-y-1">
                    <p className="text-sm font-medium leading-none">
                      Hot Reload
                    </p>
                    <p className="text-sm text-muted-foreground">
                      Patch the compiled binary to a running RPCS3's memory.
                    </p>
                  </div>
                  <Switch checked={hotReload} onCheckedChange={setHotReload} />
                </div>
                <Separator />
              </div>
            </CardContent>
            <CardFooter>
              <EnhancedButton
                className={"w-full"}
                effect={"expandIcon"}
                variant={"default"}
                icon={SiCompilerexplorer}
                iconPlacement={"right"}
                disabled={mutation.isPending}
                onClick={async () => {
                  mutation.mutate({
                    body: {
                      unitIds: [unitId],
                      replaceWorking: true,
                      hotReload: hotReload,
                    },
                  });
                }}
              >
                {mutation.isPending && (
                  <Icons.spinner
                    className="size-4 mr-2 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Compile
              </EnhancedButton>
            </CardFooter>
          </Card>
        </ResizablePanel>
      </ResizablePanelGroup>
    </div>
  );
};

export default ScriptCompiler;
