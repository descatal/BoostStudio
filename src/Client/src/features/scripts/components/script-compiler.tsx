import React from "react";
import { Monaco } from "@monaco-editor/react";
import OneDarkPro from "@/themes/onedarkpro.json";
import { useMutation, useQuery } from "@tanstack/react-query";
import {
  getApiScexDecompiledByUnitIdOptions,
  postApiScexCompileUnitsMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import { useTheme } from "@/context/theme-context";
import { toast } from "@/hooks/use-toast";
import { Option } from "@/components/ui/multiple-selector";

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
      toast({
        title: "Success",
        description: `Successfully compiled scripts to working directory!`,
      });
    },
  });

  return (
    <div className={"h-full bg-red-600"}>
      {/*<ResizablePanelGroup className={"gap-4 mb-6"} direction="horizontal">*/}
      {/*  <ResizablePanel*/}
      {/*    collapsible={true}*/}
      {/*    collapsedSize={0}*/}
      {/*    defaultSize={70}*/}
      {/*    minSize={30}*/}
      {/*  >*/}
      {/*    <Editor*/}
      {/*      className={"rounded-md border p-2"}*/}
      {/*      value={query.data ?? ""}*/}
      {/*      theme={theme === "dark" ? "OneDarkPro" : ""}*/}
      {/*      beforeMount={handleEditorDidMount}*/}
      {/*      defaultLanguage={"C++"}*/}
      {/*      options={{*/}
      {/*        wordWrap: "on",*/}
      {/*        fontSize: 14,*/}
      {/*        fontFamily: "Jetbrains-Mono",*/}
      {/*        fontLigatures: true,*/}
      {/*        readOnly: true,*/}
      {/*      }}*/}
      {/*    />*/}
      {/*  </ResizablePanel>*/}
      {/*  <ResizableHandle withHandle />*/}
      {/*  <ResizablePanel*/}
      {/*    collapsible={true}*/}
      {/*    collapsedSize={0}*/}
      {/*    defaultSize={30}*/}
      {/*    minSize={30}*/}
      {/*  >*/}
      {/*    <Card className={"h-full"}>*/}
      {/*      <CardHeader>*/}
      {/*        <CardTitle>Compile</CardTitle>*/}
      {/*        <CardDescription>*/}
      {/*          Compile units' c format code to .scex compiled binary (005.bin)*/}
      {/*          in data folder.*/}
      {/*        </CardDescription>*/}
      {/*      </CardHeader>*/}
      {/*      <CardContent>*/}
      {/*        <div className="grid gap-4">*/}
      {/*          <div className={"space-y-2"}>*/}
      {/*            <Label>Units</Label>*/}
      {/*            <UnitsSelector*/}
      {/*              disabled*/}
      {/*              className={"w-full"}*/}
      {/*              defaultValues={[unitId]}*/}
      {/*              value={selectedUnitIds}*/}
      {/*              onChange={setSelectedUnitIds}*/}
      {/*              placeholder={unitId ? undefined : "Select unit..."}*/}
      {/*            />*/}
      {/*          </div>*/}
      {/*          <div className="flex items-center space-x-4 rounded-md border p-4">*/}
      {/*            <MdMemory size={40} />*/}
      {/*            <div className="flex-1 space-y-1">*/}
      {/*              <p className="text-sm font-medium leading-none">*/}
      {/*                Hot Reload*/}
      {/*              </p>*/}
      {/*              <p className="text-sm text-muted-foreground">*/}
      {/*                Patch the compiled binary to a running RPCS3's memory.*/}
      {/*              </p>*/}
      {/*            </div>*/}
      {/*            <Switch checked={hotReload} onCheckedChange={setHotReload} />*/}
      {/*          </div>*/}
      {/*          <Separator />*/}
      {/*        </div>*/}
      {/*      </CardContent>*/}
      {/*      <CardFooter>*/}
      {/*        <EnhancedButton*/}
      {/*          className={"w-full"}*/}
      {/*          effect={"expandIcon"}*/}
      {/*          variant={"default"}*/}
      {/*          icon={SiCompilerexplorer}*/}
      {/*          iconPlacement={"right"}*/}
      {/*          disabled={mutation.isPending}*/}
      {/*          onClick={async () => {*/}
      {/*            mutation.mutate({*/}
      {/*              body: {*/}
      {/*                unitIds: [unitId],*/}
      {/*                replaceWorking: true,*/}
      {/*                hotReload: hotReload,*/}
      {/*              },*/}
      {/*            });*/}
      {/*          }}*/}
      {/*        >*/}
      {/*          {mutation.isPending && (*/}
      {/*            <Icons.spinner*/}
      {/*              className="size-4 mr-2 animate-spin"*/}
      {/*              aria-hidden="true"*/}
      {/*            />*/}
      {/*          )}*/}
      {/*          Compile*/}
      {/*        </EnhancedButton>*/}
      {/*      </CardFooter>*/}
      {/*    </Card>*/}
      {/*  </ResizablePanel>*/}
      {/*</ResizablePanelGroup>*/}
    </div>
  );
};

export default ScriptCompiler;
