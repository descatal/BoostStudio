"use client"

import * as React from "react"
import { PatchFileVersion } from "@/api/exvs"
import { SearchAssetFilePopover } from "@/pages/patches/components/tabs/components/dialog/search-asset-file-popover"
import { PatchIdNameMap } from "@/pages/patches/libs/store"
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
import { CarouselApi } from "@/components/ui/carousel"
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
import { HashInput } from "@/components/custom/hash-input"

import {
  CreatePatchFileSchema,
  UpdatePatchFileSchema,
} from "../../libs/validations"

interface PatchFileFormProps
  extends Omit<React.ComponentPropsWithRef<"form">, "onSubmit"> {
  children: React.ReactNode
  form: UseFormReturn<CreatePatchFileSchema | UpdatePatchFileSchema>
  onSubmit: (data: CreatePatchFileSchema) => void
}

export function PatchFileForm({
  form,
  onSubmit,
  children,
  className,
}: PatchFileFormProps) {
  const plugin = React.useRef(AutoHeight())
  const [api, setApi] = React.useState<CarouselApi>()

  const [hasAsset, setHasAsset] = React.useState(true)
  const [hasPath, setHasPath] = React.useState(true)

  // const [selectedTab, setSelectedTab] = React.useState<"asset" | "path">(
  //   "asset"
  // )

  return (
    <Form {...form}>
      <form
        onSubmit={(e) => {
          // to make sure the checkbox overrides everything
          if (!hasAsset) form.setValue("fileInfo", undefined)
          if (!hasPath) form.setValue("pathInfo", undefined)

          // the original form submission
          form.handleSubmit(onSubmit)(e)
        }}
        className={cn("flex flex-col gap-4", className)}
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
        <Separator />
        <div className={"max-h-[50vh] flex-row space-y-5 overflow-y-auto"}>
          <Accordion type={"multiple"}>
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
                            setHasAsset(checked as boolean)
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
                                  onHashChanged={(value) => {
                                    form.setValue("assetFileHash", value)
                                  }}
                                />
                                <SearchAssetFilePopover
                                  setAssetFile={(asset) => {
                                    if (!asset) return
                                    form.setValue("assetFileHash", undefined)
                                    // not the best way, but this will force a re-render
                                    setTimeout(function () {
                                      form.setValue("assetFileHash", asset.hash)
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
                                  ></FormField>
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
                                  ></FormField>
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
                                  ></FormField>
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
              </AccordionContent>
            </AccordionItem>
          </Accordion>
        </div>

        {/*<Carousel*/}
        {/*  plugins={[plugin.current]}*/}
        {/*  className={"w-full"}*/}
        {/*  setApi={setApi}*/}
        {/*>*/}
        {/*  <CarouselContent className={"flex items-start"}>*/}
        {/*<CarouselItem>*/}
        {/*  <Card>*/}
        {/*    <CardContent className={"flex flex-col space-y-6 p-8"}>*/}
        {/*      <div className="grid w-full items-center gap-1.5">*/}
        {/*        <Label htmlFor="file">Select File</Label>*/}
        {/*        <Input id="file" type="file" onChange={(e) => {}} />*/}
        {/*      </div>*/}
        {/*      <Label className={"text-center font-semibold"}>OR</Label>*/}
        {/*      <div>*/}
        {/*        <Button*/}
        {/*          type={"button"}*/}
        {/*          className={"w-full"}*/}
        {/*          onClick={() => {*/}
        {/*            if (!api) return*/}
        {/*            api.scrollTo(1)*/}
        {/*          }}*/}
        {/*        >*/}
        {/*          Fill In Manually*/}
        {/*        </Button>*/}
        {/*      </div>*/}
        {/*    </CardContent>*/}
        {/*  </Card>*/}
        {/*</CarouselItem>*/}
        {/*<CarouselItem>*/}
        {/*  <div className={"flex-row space-y-4"}>*/}
        {/*    <div*/}
        {/*      className={"max-h-[50vh] flex-row space-y-2 overflow-y-auto"}*/}
        {/*    >*/}
        {/*<Tabs*/}
        {/*  defaultValue="unit"*/}
        {/*  className="w-full"*/}
        {/*  value={selectedTab}*/}
        {/*  onValueChange={(e) => {*/}
        {/*    setSelectedTab(e as "asset" | "path")*/}
        {/*  }}*/}
        {/*>*/}
        {/*  <TabsList>*/}
        {/*    <TabsTrigger value="asset">Asset</TabsTrigger>*/}
        {/*    <TabsTrigger value="path">Path</TabsTrigger>*/}
        {/*  </TabsList>*/}
        {/*</TabsContent>*/}
        {/*<TabsContent value="path" className="space-y-4">*/}
        {/* Card contents go here */}
        {/*</TabsContent>*/}
        {/*</Tabs>*/}
        {/*  <TabsContent value="asset" className="space-y-4">*/}
        {/* Card contents go here */}
        {/*</div>*/}
        {/*<Separator />*/}
        {/*<Button*/}
        {/*  type={"button"}*/}
        {/*  className={"w-full"}*/}
        {/*  onClick={() => {*/}
        {/*    if (!api) return*/}
        {/*    api.scrollTo(0)*/}
        {/*  }}*/}
        {/*>*/}
        {/*  Go Back*/}
        {/*</Button>*/}
        {/*      </div>*/}
        {/*    </CarouselItem>*/}
        {/*  </CarouselContent>*/}
        {/*</Carousel>*/}
        {children}
      </form>
    </Form>
  )
}
