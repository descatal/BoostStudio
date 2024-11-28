import React, {useEffect} from "react"
import PatchInformation from "@/pages/patches/components/tabs"
import {
  PatchFileTabs,
  useCustomizePatchInformationStore,
} from "@/pages/patches/libs/store"
import {
  generatePath,
  matchPath,
  useLocation,
  useNavigate,
  useParams,
} from "react-router-dom"

import {Tabs, TabsContent, TabsList, TabsTrigger} from "@/components/ui/tabs"

const routes = ["/patches/:patchId"]

export default function PatchesPage() {
  const {selectedPatchFileVersion, setSelectedPatchFileVersion} =
    useCustomizePatchInformationStore((state) => state)

  const {pathname} = useLocation()
  const params = useParams()
  const navigate = useNavigate()

  const pathPattern = routes.find((pattern) => matchPath(pattern, pathname))

  useEffect(() => {
    setSelectedPatchFileVersion(params.patchId as PatchFileTabs)
  }, [params.patchId])

  return (
    <div className="flex-col md:flex">
      <div className="flex-1 space-y-4 p-8 pt-6">
        <div className="flex items-center justify-between space-y-2">
          <h2 className="text-3xl font-bold tracking-tight">Patches</h2>
        </div>
        <Tabs
          defaultValue="Patch6"
          className="space-y-4"
          value={selectedPatchFileVersion}
          onValueChange={(value: string) => {
            if (pathPattern === undefined) return
            const newPath = generatePath(pathPattern, {
              ...params,
              patchId: value,
            })
            navigate(
              {
                pathname: `${newPath}`,
              },
              {
                replace: true,
              }
            )
          }}
        >
          <TabsList>
            <TabsTrigger value="All">All</TabsTrigger>
            <TabsTrigger value="Base">Base</TabsTrigger>
            <TabsTrigger value="Patch1">Patch 1</TabsTrigger>
            <TabsTrigger value="Patch2">Patch 2</TabsTrigger>
            <TabsTrigger value="Patch3">Patch 3</TabsTrigger>
            <TabsTrigger value="Patch4">Patch 4</TabsTrigger>
            <TabsTrigger value="Patch5">Patch 5</TabsTrigger>
            <TabsTrigger value="Patch6">Patch 6</TabsTrigger>
          </TabsList>
          <TabsContent value="All" className="space-y-4">
            <PatchInformation/>
          </TabsContent>
          <TabsContent value="Base" className="space-y-4">
            <PatchInformation patchId={"Base"}/>
          </TabsContent>
          <TabsContent value="Patch1" className="space-y-4">
            <PatchInformation patchId={"Patch1"}/>
          </TabsContent>
          <TabsContent value="Patch2" className="space-y-4">
            <PatchInformation patchId={"Patch2"}/>
          </TabsContent>
          <TabsContent value="Patch3" className="space-y-4">
            <PatchInformation patchId={"Patch3"}/>
          </TabsContent>
          <TabsContent value="Patch4" className="space-y-4">
            <PatchInformation patchId={"Patch4"}/>
          </TabsContent>
          <TabsContent value="Patch5" className="space-y-4">
            <PatchInformation patchId={"Patch5"}/>
          </TabsContent>
          <TabsContent value="Patch6" className="space-y-4">
            <PatchInformation patchId={"Patch6"}/>
          </TabsContent>
        </Tabs>
      </div>
    </div>
  )
}
