"use client"

import * as React from "react"
import { PatchFileVersion } from "@/api/exvs"
import { SearchAssetFilePopover } from "@/pages/patches/components/tabs/components/dialog/search-asset-file-popover"
import {
  PatchIdNameMap,
  useCustomizePatchInformationStore,
} from "@/pages/patches/libs/store"
import AutoHeight from "embla-carousel-auto-height"
import { type UseFormReturn } from "react-hook-form"

import { cn } from "@/lib/utils"
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader } from "@/components/ui/card"
import {
  Carousel,
  CarouselApi,
  CarouselContent,
  CarouselItem,
} from "@/components/ui/carousel"
import { Checkbox } from "@/components/ui/checkbox"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { PopoverTrigger } from "@/components/ui/popover"
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { Separator } from "@/components/ui/separator"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { HashInput } from "@/components/custom/hash-input"

import { CreatePatchFileSchema } from "../../libs/validations"

interface CreatePatchFileFormProps
  extends Omit<React.ComponentPropsWithRef<"form">, "onSubmit"> {
  children: React.ReactNode
  form: UseFormReturn<CreatePatchFileSchema>
  onSubmit: (data: CreatePatchFileSchema) => void
}

export function CreatePatchFileForm({
  form,
  onSubmit,
  children,
  className,
}: CreatePatchFileFormProps) {
  const plugin = React.useRef(AutoHeight())
  const [api, setApi] = React.useState<CarouselApi>()

  const [hasAsset, setHasAsset] = React.useState(false)
  const [hasPath, setHasPath] = React.useState(false)

  const { selectedPatchFileVersion } = useCustomizePatchInformationStore(
    (state) => state
  )

  const [selectedTab, setSelectedTab] = React.useState<"asset" | "path">(
    "asset"
  )

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className={cn("flex flex-col gap-4", className)}
      >
        <FormField
          control={form.control}
          name="tblId"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Table</FormLabel>
              <Select
                onValueChange={field.onChange}
                defaultValue={selectedPatchFileVersion ?? field.value}
              >
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
        <Separator />
        <Label>File Information</Label>
        <Carousel
          plugins={[plugin.current]}
          className={"w-full"}
          setApi={setApi}
        >
          <CarouselContent className={"flex items-start"}>
            <CarouselItem>
              <Card>
                <CardContent className={"flex flex-col space-y-6 p-8"}>
                  <div className="grid w-full items-center gap-1.5">
                    <Label htmlFor="picture">Select File</Label>
                    <Input id="picture" type="file" />
                  </div>
                  <Label className={"text-center font-semibold"}>OR</Label>
                  <div>
                    <Button
                      type={"button"}
                      className={"w-full"}
                      onClick={() => {
                        if (!api) return
                        api.scrollTo(1)
                      }}
                    >
                      Fill In Manually
                    </Button>
                  </div>
                </CardContent>
              </Card>
            </CarouselItem>
            <CarouselItem>
              <div className={"flex-row space-y-4"}>
                <div
                  className={"max-h-[50vh] flex-row space-y-2 overflow-y-auto"}
                >
                  <Tabs
                    defaultValue="unit"
                    className="w-full"
                    value={selectedTab}
                    onValueChange={(e) => {
                      setSelectedTab(e as "asset" | "path")
                    }}
                  >
                    <TabsList>
                      <TabsTrigger value="asset">Asset</TabsTrigger>
                      <TabsTrigger value="path">Path</TabsTrigger>
                    </TabsList>
                    <TabsContent value="asset" className="space-y-4">
                      <Card>
                        <CardHeader>
                          <div className={"flex justify-between"}>
                            <div className="flex items-center justify-end space-x-2">
                              <Checkbox
                                id={"asset"}
                                checked={hasAsset}
                                onCheckedChange={() => setHasAsset(!hasAsset)}
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
                        <Separator />
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
                                      />
                                      <SearchAssetFilePopover
                                        setAssetFile={(asset) => {
                                          if (!asset) return
                                          form.setValue(
                                            "assetFileHash",
                                            undefined
                                          )
                                          // not the best way, but this will force a re-render
                                          setTimeout(function () {
                                            form.setValue(
                                              "assetFileHash",
                                              asset.hash
                                            )
                                          }, 5)
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
                                    onValueChange={field.onChange}
                                    defaultValue={selectedPatchFileVersion}
                                  >
                                    <FormControl>
                                      <SelectTrigger className="capitalize">
                                        <SelectValue placeholder="Select Version" />
                                      </SelectTrigger>
                                    </FormControl>
                                    <SelectContent>
                                      <SelectGroup
                                        defaultValue={form.getValues("tblId")}
                                      >
                                        {Object.values(PatchFileVersion).map(
                                          (version) => (
                                            <SelectItem
                                              key={version}
                                              value={version}
                                              className="capitalize"
                                            >
                                              {version}
                                            </SelectItem>
                                          )
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
                                      <div
                                        className={"mt-5 flex-row space-y-3"}
                                      >
                                        <FormField
                                          control={form.control}
                                          name={"fileInfo.size1"}
                                          render={({ field }) => (
                                            <FormItem>
                                              <FormLabel>Size 1</FormLabel>
                                              <FormControl>
                                                <Input
                                                  disabled={!hasAsset}
                                                  placeholder={"Size 1"}
                                                  {...field}
                                                ></Input>
                                              </FormControl>
                                            </FormItem>
                                          )}
                                        ></FormField>
                                        <FormField
                                          control={form.control}
                                          name={"fileInfo.size2"}
                                          render={({ field }) => (
                                            <FormItem>
                                              <FormLabel>Size 2</FormLabel>
                                              <FormControl>
                                                <Input
                                                  disabled={!hasAsset}
                                                  placeholder={"Size 2"}
                                                  {...field}
                                                ></Input>
                                              </FormControl>
                                            </FormItem>
                                          )}
                                        ></FormField>
                                        <FormField
                                          control={form.control}
                                          name={"fileInfo.size3"}
                                          render={({ field }) => (
                                            <FormItem>
                                              <FormLabel>Size 3</FormLabel>
                                              <FormControl>
                                                <Input
                                                  disabled={!hasAsset}
                                                  placeholder={"Size 3"}
                                                  {...field}
                                                ></Input>
                                              </FormControl>
                                            </FormItem>
                                          )}
                                        ></FormField>
                                        <FormField
                                          control={form.control}
                                          name={"fileInfo.size4"}
                                          render={({ field }) => (
                                            <FormItem>
                                              <FormLabel>Size 4</FormLabel>
                                              <FormControl>
                                                <Input
                                                  disabled={!hasAsset}
                                                  placeholder={"Size 4"}
                                                  {...field}
                                                ></Input>
                                              </FormControl>
                                            </FormItem>
                                          )}
                                        ></FormField>
                                      </div>
                                    </CardContent>
                                  </Card>
                                </AccordionContent>
                              </AccordionItem>
                            </Accordion>
                          </div>
                        </CardContent>
                      </Card>
                    </TabsContent>
                    <TabsContent value="path" className="space-y-4">
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
                        <Separator />
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
                                      placeholder={`${PatchIdNameMap[selectedPatchFileVersion]}/`}
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
                                  <FormLabel>Path</FormLabel>
                                  <FormControl>
                                    <Input
                                      disabled={!hasPath}
                                      type={"number"}
                                      placeholder={`Leave blank for auto increment`}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                          </div>
                        </CardContent>
                      </Card>
                    </TabsContent>
                  </Tabs>
                </div>
                <Separator />
                <Button
                  type={"button"}
                  className={"w-full"}
                  onClick={() => {
                    if (!api) return
                    api.scrollTo(0)
                  }}
                >
                  Go Back
                </Button>
              </div>
            </CarouselItem>
          </CarouselContent>
        </Carousel>
        {children}
      </form>
    </Form>
  )
}
