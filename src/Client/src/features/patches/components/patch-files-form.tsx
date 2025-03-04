import * as React from "react";
import { PatchFileVersion } from "@/api/exvs";
import {
  CreatePatchFileSchema,
  UpdatePatchFileSchema,
} from "@/features/patches/libs/validations";
import { SearchAssetFilePopover } from "@/pages/patches/components/tabs/components/dialog/search-asset-file-popover";
import { type UseFormReturn } from "react-hook-form";

import { cn } from "@/lib/utils";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader } from "@/components/ui/card";
import { Checkbox } from "@/components/ui/checkbox";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { PopoverTrigger } from "@/components/ui/popover";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { HashInput } from "@/components/custom/hash-input";
import { PatchIdNameMap } from "@/features/patches/libs/constants";

interface PatchFilesFormProps
  extends Omit<React.ComponentPropsWithRef<"form">, "onSubmit"> {
  children: React.ReactNode;
  form: UseFormReturn<CreatePatchFileSchema | UpdatePatchFileSchema>;
  onSubmit: (data: CreatePatchFileSchema) => void;
}

export function PatchFilesForm({
  form,
  onSubmit,
  children,
  className,
}: PatchFilesFormProps) {
  const [hasAsset, setHasAsset] = React.useState(true);
  const [hasPath, setHasPath] = React.useState(true);

  return (
    <Form {...form}>
      <form
        onSubmit={(e) => {
          // to make sure the checkbox overrides everything
          if (!hasAsset) form.setValue("fileInfo", undefined);
          if (!hasPath) form.setValue("pathInfo", undefined);

          // the original form submission
          form.handleSubmit(onSubmit)(e);
        }}
        className={cn(
          "flex max-h-screen flex-col gap-4 overflow-auto px-4",
          className,
        )}
      >
        <FormField
          control={form.control}
          name="tblId"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Table</FormLabel>
              <Select onValueChange={field.onChange} defaultValue={field.value}>
                <FormControl>
                  <SelectTrigger className="capitalize">
                    <SelectValue placeholder="Select Table Version" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  <SelectGroup>
                    {Object.values(PatchFileVersion).map((version) => (
                      <SelectItem
                        key={version}
                        value={version}
                        className="capitalize"
                      >
                        {version}
                      </SelectItem>
                    ))}
                  </SelectGroup>
                </SelectContent>
              </Select>
              <FormMessage />
            </FormItem>
          )}
        />
        <div className={"flex-row space-y-5"}>
          <Accordion type={"multiple"} className={"w-full"}>
            <AccordionItem value="path">
              <AccordionTrigger>
                <Label>Path Information</Label>
              </AccordionTrigger>
              <AccordionContent>
                <Card>
                  <CardHeader>
                    <div className={"flex justify-between"}>
                      <div className="flex items-center justify-end space-x-2">
                        <Checkbox
                          id={"path"}
                          checked={hasPath}
                          onCheckedChange={() => setHasPath(!hasPath)}
                        />
                        <label
                          htmlFor="path"
                          className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                        >
                          Has Path
                        </label>
                      </div>
                    </div>
                  </CardHeader>
                  <CardContent>
                    <div className={`mt-4 space-y-3`}>
                      <FormField
                        control={form.control}
                        name={"pathInfo.path"}
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Path</FormLabel>
                            <FormControl>
                              <Input
                                disabled={!hasPath}
                                placeholder={`${PatchIdNameMap[form.getValues().tblId] ?? ""}/`}
                                {...field}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <FormField
                        control={form.control}
                        name={"pathInfo.order"}
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Order</FormLabel>
                            <FormControl>
                              <Input
                                {...field}
                                disabled={!hasPath}
                                placeholder={`Leave blank for auto increment`}
                                type="number"
                                min={0}
                                value={field.value ?? ""}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                    </div>
                  </CardContent>
                </Card>
              </AccordionContent>
            </AccordionItem>
            <AccordionItem value="file">
              <AccordionTrigger>
                <Label>File Information</Label>
              </AccordionTrigger>
              <AccordionContent>
                <Card>
                  <CardHeader>
                    <div className={"flex justify-between"}>
                      <div className="flex items-center justify-end space-x-2">
                        <Checkbox
                          id={"asset"}
                          checked={hasAsset}
                          onCheckedChange={(checked) => {
                            setHasAsset(checked as boolean);
                          }}
                        />
                        <label
                          htmlFor="asset"
                          className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                        >
                          Has Asset File
                        </label>
                      </div>
                    </div>
                  </CardHeader>
                  <CardContent>
                    <div className={`mt-4 space-y-3`}>
                      <FormField
                        control={form.control}
                        name={"assetFileHash"}
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Hash</FormLabel>
                            <FormControl>
                              <div className={"flex flex-row space-x-2"}>
                                <HashInput
                                  disabled={!hasAsset}
                                  initialMode={"hex"}
                                  initialValue={field.value}
                                  placeholder="Enter Hash"
                                  {...field}
                                  value={field.value ?? undefined}
                                  onHashChanged={(value) => {
                                    form.setValue(
                                      "assetFileHash",
                                      value ?? null,
                                    );
                                  }}
                                />
                                <SearchAssetFilePopover
                                  setAssetFile={(asset) => {
                                    if (!asset) return;
                                    form.setValue("assetFileHash", null);
                                    // not the best way, but this will force a re-render
                                    setTimeout(function () {
                                      form.setValue(
                                        "assetFileHash",
                                        asset.hash,
                                      );
                                    }, 5);
                                  }}
                                >
                                  <PopoverTrigger asChild>
                                    <Button
                                      disabled={!hasAsset}
                                      type={"button"}
                                    >
                                      Search...
                                    </Button>
                                  </PopoverTrigger>
                                </SearchAssetFilePopover>
                              </div>
                            </FormControl>
                          </FormItem>
                        )}
                      />
                      <FormField
                        control={form.control}
                        name={"fileInfo.version"}
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Version</FormLabel>
                            <Select
                              disabled={!hasAsset}
                              value={field.value}
                              onValueChange={field.onChange}
                              defaultValue={form.getValues("tblId")}
                            >
                              <FormControl>
                                <SelectTrigger className="capitalize">
                                  <SelectValue placeholder="Select Version" />
                                </SelectTrigger>
                              </FormControl>
                              <SelectContent>
                                <SelectGroup>
                                  {Object.values(PatchFileVersion).map(
                                    (version) => (
                                      <SelectItem
                                        key={version}
                                        value={version}
                                        className="capitalize"
                                      >
                                        {version}
                                      </SelectItem>
                                    ),
                                  )}
                                </SelectGroup>
                              </SelectContent>
                            </Select>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <Accordion
                        type="single"
                        defaultValue={"sizes"}
                        collapsible
                      >
                        <AccordionItem value="sizes">
                          <AccordionTrigger>
                            <Label>File Sizes</Label>
                          </AccordionTrigger>
                          <AccordionContent>
                            <Card>
                              <CardContent>
                                <div className={"mt-5 flex-row space-y-3"}>
                                  <FormField
                                    control={form.control}
                                    name={"fileInfo.size1"}
                                    render={({ field }) => (
                                      <FormItem>
                                        <FormLabel>Size 1</FormLabel>
                                        <FormControl>
                                          <Input
                                            {...field}
                                            placeholder={"Size 1"}
                                            disabled={!hasAsset}
                                            type="number"
                                            min={0}
                                            value={field.value ?? ""}
                                          ></Input>
                                        </FormControl>
                                      </FormItem>
                                    )}
                                  />
                                  <FormField
                                    control={form.control}
                                    name={"fileInfo.size2"}
                                    render={({ field }) => (
                                      <FormItem>
                                        <FormLabel>Size 2</FormLabel>
                                        <FormControl>
                                          <Input
                                            {...field}
                                            disabled={!hasAsset}
                                            placeholder={"Size 2"}
                                            type="number"
                                            min={0}
                                            value={field.value ?? ""}
                                          ></Input>
                                        </FormControl>
                                      </FormItem>
                                    )}
                                  />
                                  <FormField
                                    control={form.control}
                                    name={"fileInfo.size3"}
                                    render={({ field }) => (
                                      <FormItem>
                                        <FormLabel>Size 3</FormLabel>
                                        <FormControl>
                                          <Input
                                            {...field}
                                            disabled={!hasAsset}
                                            placeholder={"Size 3"}
                                            type="number"
                                            min={0}
                                            value={field.value ?? ""}
                                          ></Input>
                                        </FormControl>
                                      </FormItem>
                                    )}
                                  />
                                  <FormField
                                    control={form.control}
                                    name={"fileInfo.size4"}
                                    render={({ field }) => (
                                      <FormItem>
                                        <FormLabel>Size 4</FormLabel>
                                        <FormControl>
                                          <Input
                                            {...field}
                                            disabled={!hasAsset}
                                            placeholder={"Size 4"}
                                            type="number"
                                            min={0}
                                            value={field.value ?? ""}
                                          ></Input>
                                        </FormControl>
                                      </FormItem>
                                    )}
                                  />
                                </div>
                              </CardContent>
                            </Card>
                          </AccordionContent>
                        </AccordionItem>
                      </Accordion>
                    </div>
                  </CardContent>
                </Card>
              </AccordionContent>
            </AccordionItem>
          </Accordion>
        </div>
        {children}
      </form>
    </Form>
  );
}
