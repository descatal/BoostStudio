import React, { useState } from "react";
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
import { DownloadIcon, Loader2, Upload, X } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useMutation } from "@tanstack/react-query";
import { toast } from "@/hooks/use-toast";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { LuPackageOpen } from "react-icons/lu";
import { GetProblemDetails } from "@/lib/errors";
import { postApiFhmUnpackMutation } from "@/api/exvs/@tanstack/react-query.gen";

// can deprecate this out in favor of zod once this is implemented:
// https://github.com/hey-api/openapi-ts/pull/1616
const formSchema = z.object({
  files: z
    .array(z.custom<File>())
    .min(1, "Please select at least one file")
    .max(1, "Please select only 1 file")
    .refine((files) => files.every((file) => file.size <= 5 * 1024 * 1024), {
      message: "File size must be less than 5MB",
      path: ["files"],
    }),
});

type FormValues = z.infer<typeof formSchema>;

const UnpackFhmForm = () => {
  const [convertedFiles, setConvertedFiles] = useState<File[]>([]);

  const form = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      files: [],
    },
  });

  const unpackMutation = useMutation({
    ...postApiFhmUnpackMutation(),
    onMutate: () => {
      toast({
        title: "Unpacking",
        description: "Unpacking the provided file into .tar format...",
      });
    },
    onSuccess: (data) => {
      toast({
        title: "Successful!",
        description: "Packed FHM file successfully!",
      });

      const fileName = form.getValues("files")[0]?.name ?? "unpacked.tar";
      const fileNameWithoutExt = fileName.substring(
        0,
        fileName.lastIndexOf("."),
      );

      const file = new File([data], `${fileNameWithoutExt}.tar`, {
        type: data.type,
      });
      setConvertedFiles([file]);
    },
    onSettled: async (_, error) => {
      if (error) {
        const problemDetails = await GetProblemDetails(error);
        form.setError("files", {
          message: problemDetails?.title ?? error.message,
        });
      }
    },
  });

  const onSubmit = React.useCallback(async (data: FormValues) => {
    unpackMutation.mutate({
      body: {
        file: data.files[0],
      },
    });
  }, []);

  return (
    <>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="w-full">
          <FormField
            control={form.control}
            name="files"
            render={({ field, fieldState }) => (
              <FormItem>
                <FormLabel>Attachments</FormLabel>
                <FormControl>
                  <FileUpload
                    value={field.value}
                    onValueChange={field.onChange}
                    accept=".fhm"
                    maxFiles={1}
                    maxSize={100 * 1024 * 1024}
                    invalid={fieldState.invalid}
                    onFileReject={(_, message) => {
                      form.setError("files", {
                        message,
                      });
                    }}
                  >
                    <FileUploadDropzone>
                      <div className="flex flex-col items-center gap-1">
                        <div className="flex items-center justify-center rounded-full border p-2.5">
                          <Upload className="size-6 text-muted-foreground" />
                        </div>
                        <p className="font-medium text-sm">
                          Drag & drop files here
                        </p>
                        <p className="text-muted-foreground text-xs">
                          Or click to browse (max 1 files, up to 100MB each)
                        </p>
                      </div>
                      <FileUploadTrigger asChild>
                        <Button
                          variant="outline"
                          size="sm"
                          className="mt-2 w-fit"
                        >
                          Browse files
                        </Button>
                      </FileUploadTrigger>
                    </FileUploadDropzone>
                    <FileUploadList>
                      {field.value.map((file, index) => (
                        <FileUploadItem key={index} value={file}>
                          <FileUploadItemPreview />
                          <FileUploadItemMetadata />
                          {convertedFiles[0] && (
                            <Button
                              variant="ghost"
                              size="icon"
                              className="size-7"
                              title={"Download converted file"}
                              type={"button"}
                              onClick={() => {
                                const convertedFile = convertedFiles[0];
                                const url = URL.createObjectURL(convertedFile);
                                const a = document.createElement("a");
                                a.href = url;
                                a.download = convertedFile.name;
                                document.body.appendChild(a);
                                a.click();
                                document.body.removeChild(a);
                                URL.revokeObjectURL(url);
                              }}
                            >
                              <DownloadIcon />
                            </Button>
                          )}
                          <FileUploadItemDelete asChild>
                            <Button
                              variant="ghost"
                              size="icon"
                              className="size-7"
                              title={"Remove file"}
                            >
                              <X />
                            </Button>
                          </FileUploadItemDelete>
                        </FileUploadItem>
                      ))}
                    </FileUploadList>
                  </FileUpload>
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <div className={"flex justify-end mt-3"}>
            <EnhancedButton
              disabled={unpackMutation.isPending}
              className={"w-1/12 min-w-fit"}
              icon={LuPackageOpen}
              iconPlacement={"right"}
              effect={"expandIcon"}
            >
              {unpackMutation.isPending && (
                <Loader2 className="animate-spin mr-2" />
              )}
              Unpack
            </EnhancedButton>
          </div>
        </form>
      </Form>
      <FileUpload>
        <FileUploadList>
          <FileUploadItem value={convertedFiles[0]!}>
            <FileUploadItemPreview />
            <FileUploadItemMetadata />
            <FileUploadItemDelete asChild>
              <Button variant="ghost" size="icon" className="size-7">
                <X />
              </Button>
            </FileUploadItemDelete>
          </FileUploadItem>
        </FileUploadList>
      </FileUpload>
    </>
  );
};

export default UnpackFhmForm;
