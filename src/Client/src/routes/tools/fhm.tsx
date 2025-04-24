import { createFileRoute } from "@tanstack/react-router";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import React from "react";
import {
  FileUpload,
  FileUploadDropzone,
  FileUploadItem,
  FileUploadItemDelete,
  FileUploadItemMetadata,
  FileUploadItemPreview,
  FileUploadList,
  FileUploadTrigger,
} from "@/components/ui/file-upload";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { ArrowRightIcon } from "@radix-ui/react-icons";
import { useMutation } from "@tanstack/react-query";
import { PostApiFhmPackRequest, ResponseError } from "@/api/exvs";
import { fhmApi } from "@/api/api";
import { useToast } from "@/hooks/use-toast";
import { Button } from "@/components/ui/button";
import { Upload, X } from "lucide-react";
import PackFhmForm from "@/features/fhm/components/pack-fhm-form";

export const Route = createFileRoute("/tools/fhm")({
  component: RouteComponent,
});

function RouteComponent() {
  const { toast } = useToast();
  const [packFilesValid, setPackFilesValid] = React.useState(true);
  const [packFiles, setPackFiles] = React.useState<File[]>([]);
  const [unpackFiles, setUnpackFiles] = React.useState<File[]>([]);

  const onFileValidate = React.useCallback(
    (file: File): string | null => {
      // Validate max files
      if (packFiles.length >= 1) {
        return "You can only upload up to 1 file";
      }

      // Validate file type (only images)
      if (!file.type.startsWith("image/")) {
        return "Only image files are allowed";
      }

      // Validate file size (max 100MB)
      const MAX_SIZE = 100 * 1024 * 1024; // 100MB
      if (file.size > MAX_SIZE) {
        return `File size must be less than ${MAX_SIZE / (1024 * 1024)}MB`;
      }

      return null;
    },
    [packFiles],
  );

  const packMutation = useMutation({
    mutationFn: (options: PostApiFhmPackRequest) => {
      if (!options.file) throw new Error("No upload file found");
      toast({
        title: "Packing",
        description: "Packing the provided file into .fhm format...",
      });
      return fhmApi.postApiFhmPack(options);
    },
    onSuccess: () => {
      toast({
        title: "Pack successful!",
        description: "Packed FHM file successfully!",
      });
    },
    onSettled: async (data, error) => {
      if (error) {
        setPackFilesValid(false);
        let errorMessage = error?.message ?? "Unspecified error!";

        // @ts-ignore
        if (error?.response instanceof Response) {
          // @ts-ignore
          const response = error.response as Response;
          errorMessage = (await response.json()).title;
        }

        toast({
          title: "Error",
          description: errorMessage,
          variant: "destructive",
        });
      }
    },
  });

  return (
    <>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-3xl font-bold tracking-tight">Fhm</h2>
      </div>
      <label className="text-sm text-muted-foreground">
        Pack / Unpack .fhm file
      </label>
      <Tabs defaultValue={"pack"}>
        <TabsList>
          <TabsTrigger value="pack">Pack</TabsTrigger>
          <TabsTrigger value="unpack">Unpack</TabsTrigger>
        </TabsList>
        <TabsContent className={"w-full"} value={"pack"}>
          <div className={"flex flex-col gap-4"}>
            <Card>
              <CardHeader>
                <CardTitle>Pack Assets</CardTitle>
                <CardDescription>
                  Pack asset files to .fhm container format.
                </CardDescription>
              </CardHeader>
              <CardContent>
                <PackFhmForm />
              </CardContent>
            </Card>
          </div>
        </TabsContent>
        <TabsContent className={"w-full"} value={"unpack"}>
          <div className={"flex flex-col gap-4"}>
            <Card>
              <CardHeader>
                <CardTitle>Unpack Assets</CardTitle>
                <CardDescription>
                  Unpack asset files from .fhm container format.
                </CardDescription>
              </CardHeader>
              <CardContent>
                {/*<FileUpload*/}
                {/*  accept={{*/}
                {/*    "application/fhm": [".fhm", ".pac"],*/}
                {/*  }}*/}
                {/*/>*/}
              </CardContent>
            </Card>
            <div className={"flex justify-end"}>
              <EnhancedButton
                className={"w-1/12 min-w-fit"}
                icon={ArrowRightIcon}
                iconPlacement={"right"}
                effect={"ringHover"}
              >
                Unpack
              </EnhancedButton>
            </div>
          </div>
        </TabsContent>
      </Tabs>
    </>
  );
}
