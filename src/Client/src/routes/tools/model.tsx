import { createFileRoute } from "@tanstack/react-router";
import ModelViewer from "@/components/model-viewer.tsx";
import React, { useRef, useState } from "react";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label.tsx";
import { Download, FileCode, Loader2, Upload, X } from "lucide-react";
import { Card } from "@/components/ui/card.tsx";
import { postApiNdp3Mutation } from "@/api/exvs/@tanstack/react-query.gen.ts";
import { toast } from "sonner";
import { useMutation } from "@tanstack/react-query";
import { GetProblemDetails } from "@/features/errors/toast-errors.tsx";

export const Route = createFileRoute("/tools/model")({
  component: RouteComponent,
});

function RouteComponent() {
  const [modelFile, setModelFile] = useState<File | null>(null);
  const [skeletonFile, setSkeletonFile] = useState<File | null>(null);
  const [convertedModel, setConvertedModel] = useState<string | null>(null);
  const [isConverting, setIsConverting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const modelInputRef = useRef<HTMLInputElement>(null);
  const skeletonInputRef = useRef<HTMLInputElement>(null);

  const convertMutation = useMutation({
    ...postApiNdp3Mutation(),
    onMutate: () => {
      toast("Converting", {
        description:
          "Converting the provided model file(s) into .glb format...",
      });
    },
    onSuccess: (data) => {
      toast("Successful!", {
        description: "Converted model file successfully!",
      });

      const fileName = modelFile?.name ?? "converted.glb";
      const fileNameWithoutExt = fileName.substring(
        0,
        fileName.lastIndexOf("."),
      );

      const file = new File([data], `${fileNameWithoutExt}.glb`, {
        type: data.type,
      });
      const url = URL.createObjectURL(file);
      setConvertedModel(url);
    },
    onSettled: async (_, error) => {
      if (error) {
        const problemDetails = await GetProblemDetails(error);
        setError(problemDetails?.title ?? error.message);
      }
    },
  });

  const handleModelFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      const ext = file.name.split(".").pop()?.toLowerCase();
      if (ext === "ndp3" || ext === "nud") {
        setModelFile(file);
        setError(null);
      } else {
        setError("Please select a .ndp3 or .nud file");
      }
    }
  };

  const handleSkeletonFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      const ext = file.name.split(".").pop()?.toLowerCase();
      if (ext === "vbn") {
        setSkeletonFile(file);
        setError(null);
      } else {
        setError("Please select a .vbn file");
      }
    }
  };

  const handleConvert = async () => {
    if (!modelFile) {
      setError("Please select a model file");
      return;
    }

    setIsConverting(true);
    setError(null);

    try {
      convertMutation.mutate({
        body: {
          ndp3File: modelFile,
          vbnFile: skeletonFile ?? undefined,
        },
      });
    } catch (err) {
      setError("Conversion failed. Please try again.");
      console.error(err);
    } finally {
      setIsConverting(false);
    }
  };

  const handleDownload = () => {
    if (convertedModel) {
      const link = document.createElement("a");
      link.href = convertedModel;
      link.download = `${modelFile?.name.split(".")[0] || "model"}.glb`;
      link.click();
    }
  };

  const handleReset = () => {
    setModelFile(null);
    setSkeletonFile(null);
    setConvertedModel(null);
    setError(null);
    if (modelInputRef.current) modelInputRef.current.value = "";
    if (skeletonInputRef.current) skeletonInputRef.current.value = "";
  };

  return (
    <div className="flex h-full bg-background">
      {/* Left Panel - Controls */}
      <div className="border-r border-border p-6 flex flex-col">
        <div className="mb-8">
          <h1 className="text-2xl font-bold text-card-foreground mb-2">
            Model Converter
          </h1>
          <p className="text-sm text-muted-foreground">
            Convert game model files to GLTF format
          </p>
        </div>

        <div className="flex-1 space-y-6 overflow-y-auto">
          {/* Model File Upload */}
          <div className="space-y-2">
            <Label htmlFor="model-file" className="text-sm font-medium">
              Model File <span className="text-destructive">*</span>
            </Label>
            <p className="text-xs text-muted-foreground mb-2">
              Required: .ndp3 or .nud file
            </p>
            <div className="relative">
              <input
                ref={modelInputRef}
                id="model-file"
                type="file"
                accept=".ndp3,.nud"
                onChange={handleModelFileChange}
                className="hidden"
              />
              <Button
                variant="outline"
                className="w-full justify-start text-left font-normal bg-transparent"
                onClick={() => modelInputRef.current?.click()}
              >
                <Upload className="mr-2 h-4 w-4" />
                {modelFile ? modelFile.name : "Choose model file..."}
              </Button>
              {modelFile && (
                <Button
                  variant="ghost"
                  size="icon"
                  className="absolute right-1 top-1 h-7 w-7"
                  onClick={(e) => {
                    e.stopPropagation();
                    setModelFile(null);
                    if (modelInputRef.current) modelInputRef.current.value = "";
                  }}
                >
                  <X className="h-4 w-4" />
                </Button>
              )}
            </div>
          </div>

          {/* Skeleton File Upload */}
          <div className="space-y-2">
            <Label htmlFor="skeleton-file" className="text-sm font-medium">
              Skeleton File{" "}
              <span className="text-muted-foreground">(Optional)</span>
            </Label>
            <p className="text-xs text-muted-foreground mb-2">
              Optional: .vbn skeleton file
            </p>
            <div className="relative">
              <input
                ref={skeletonInputRef}
                id="skeleton-file"
                type="file"
                accept=".vbn"
                onChange={handleSkeletonFileChange}
                className="hidden"
              />
              <Button
                variant="outline"
                className="w-full justify-start text-left font-normal bg-transparent"
                onClick={() => skeletonInputRef.current?.click()}
              >
                <Upload className="mr-2 h-4 w-4" />
                {skeletonFile ? skeletonFile.name : "Choose skeleton file..."}
              </Button>
              {skeletonFile && (
                <Button
                  variant="ghost"
                  size="icon"
                  className="absolute right-1 top-1 h-7 w-7"
                  onClick={(e) => {
                    e.stopPropagation();
                    setSkeletonFile(null);
                    if (skeletonInputRef.current)
                      skeletonInputRef.current.value = "";
                  }}
                >
                  <X className="h-4 w-4" />
                </Button>
              )}
            </div>
          </div>

          {/* Error Message */}
          {error && (
            <div className="p-3 rounded-md bg-destructive/10 border border-destructive/20">
              <p className="text-sm text-destructive">{error}</p>
            </div>
          )}

          {/* File Info */}
          {modelFile && (
            <Card className="p-4 bg-muted/50">
              <div className="flex items-start gap-3">
                <FileCode className="h-5 w-5 text-primary mt-0.5" />
                <div className="flex-1 min-w-0">
                  <p className="text-sm font-medium text-card-foreground truncate">
                    {modelFile.name}
                  </p>
                  <p className="text-xs text-muted-foreground">
                    {(modelFile.size / 1024).toFixed(2)} KB
                  </p>
                  {skeletonFile && (
                    <p className="text-xs text-muted-foreground mt-1">
                      + {skeletonFile.name}
                    </p>
                  )}
                </div>
              </div>
            </Card>
          )}

          {/* Convert Button */}
          <Button
            className="w-full"
            size="lg"
            onClick={handleConvert}
            disabled={!modelFile || isConverting}
          >
            {isConverting ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                Converting...
              </>
            ) : (
              "Convert to GLTF"
            )}
          </Button>

          {/* Download & Reset Buttons */}
          {convertedModel && (
            <div className="space-y-2">
              <Button
                variant="outline"
                className="w-full bg-transparent"
                size="lg"
                onClick={handleDownload}
              >
                <Download className="mr-2 h-4 w-4" />
                Download GLB
              </Button>
              <Button variant="ghost" className="w-full" onClick={handleReset}>
                Reset
              </Button>
            </div>
          )}
        </div>

        {/* Footer Info */}
        <div className="mt-6 pt-6 border-t border-border">
          <p className="text-xs text-muted-foreground">
            Supported formats: .ndp3, .nud (model) • .vbn (skeleton)
          </p>
        </div>
      </div>

      {/* Right Panel - 3D Viewer */}
      <div className="flex-1 flex flex-col">
        <div className="border-b border-border bg-card px-6 py-4">
          <h2 className="text-lg font-semibold text-card-foreground">
            Preview
          </h2>
          <p className="text-sm text-muted-foreground">
            {convertedModel
              ? "Converted model preview"
              : "Upload and convert a model to preview"}
          </p>
        </div>
        <div className="flex-1 relative">
          <ModelViewer modelUrl={convertedModel} />
        </div>
      </div>
    </div>
  );
}
