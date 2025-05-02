import React from "react";
import { Editor, Monaco } from "@monaco-editor/react";
import { useApiDecompiledScexByUnitId } from "@/features/scripts/api/get-decompiled-scex-by-unit-id";
import OneDarkPro from "@/themes/onedarkpro.json";
import UnitsSelector from "@/features/units/components/units-selector";

interface ScriptViewerProps {
  unitId: number;
}

const ScriptViewer = ({ unitId }: ScriptViewerProps) => {
  const data = useApiDecompiledScexByUnitId({
    unitId: unitId,
  }).data;

  const handleEditorDidMount = (monaco: Monaco) => {
    monaco.editor.defineTheme("OneDarkPro", {
      base: "vs-dark",
      inherit: true,
      ...OneDarkPro,
    });
  };

  return (
    <>
      <UnitsSelector defaultValues={unitId ? [unitId] : undefined} />
      <Editor
        value={data}
        theme="OneDarkPro"
        beforeMount={handleEditorDidMount}
        defaultLanguage={"C++"}
        options={{
          fontSize: 14,
          fontFamily: "Jetbrains-Mono",
          fontLigatures: true,
        }}
      />
    </>
  );
};

export default ScriptViewer;
