import { createFileRoute } from '@tanstack/react-router'

import { useRestaurant, useRestaurantMenu } from '../../features/restaurants/queries'
import { useDeleteAllMenuItems, useDeleteRestaurant } from '../../features/restaurants/mutations'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card'
import { Button } from '../../components/ui/button'
import { Badge } from '../../components/ui/badge'
import { Separator } from '../../components/ui/separator'
import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from '../../components/ui/alert-dialog'
import { Trash2, UtensilsCrossed } from 'lucide-react'

export const Route = createFileRoute('/restaurants/$id')({
    component: RestaurantDetail,
})

function RestaurantDetail() {
    const { id: idParam } = Route.useParams()
    const id = Number(idParam)


    // Fetch and cache the restaurant by a stable key.
    // React Query handles loading/error state and caching
    // de-dupes requests, and can refetch in the background.
    const { data } = useRestaurant(id)
    const { data: menu } = useRestaurantMenu(id)

    // Delete the restaurant, then invalidate the restaurants list so it refetches.
    const delRestaurant = useDeleteRestaurant(id)
    // Delete all menu items, then invalidate this restaurant’s menu cache.
    const delAllMenu = useDeleteAllMenuItems(id)

    // Normalize potential response shapes from the API.
    const r = (data as any)?.restaurant ?? (data as any)
    const menuItems = (menu as any)?.items ?? (menu as any)?.menuItems ?? []

    return (
        <div className="space-y-6">
            <Card>
                <CardHeader>
                    <div className="flex items-start justify-between">
                        <div className="space-y-1">
                            <CardTitle className="text-3xl">{r?.name}</CardTitle>
                            <CardDescription>{r?.description}</CardDescription>
                            {r?.category && <Badge className="mt-2">{r?.category}</Badge>}
                        </div>
                        <div className="flex gap-2">
                            <AlertDialog>
                                <AlertDialogTrigger asChild>
                                    <Button variant="outline" size="sm">
                                        <Trash2 className="h-4 w-4 mr-2" />
                                        Delete All Menu
                                    </Button>
                                </AlertDialogTrigger>
                                <AlertDialogContent>
                                    <AlertDialogHeader>
                                        <AlertDialogTitle>Delete all menu items?</AlertDialogTitle>
                                        <AlertDialogDescription>
                                            This will permanently delete all menu items for this restaurant.
                                        </AlertDialogDescription>
                                    </AlertDialogHeader>
                                    <AlertDialogFooter>
                                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                                        <AlertDialogAction onClick={() => delAllMenu.mutate()} disabled={!id || Number.isNaN(id)}>Delete</AlertDialogAction>
                                    </AlertDialogFooter>
                                </AlertDialogContent>
                            </AlertDialog>

                            <AlertDialog>
                                <AlertDialogTrigger asChild>
                                    <Button variant="destructive" size="sm">
                                        <Trash2 className="h-4 w-4 mr-2" />
                                        Delete Restaurant
                                    </Button>
                                </AlertDialogTrigger>
                                <AlertDialogContent>
                                    <AlertDialogHeader>
                                        <AlertDialogTitle>Delete restaurant?</AlertDialogTitle>
                                        <AlertDialogDescription>
                                            This action cannot be undone. This will permanently delete the restaurant and all associated data.
                                        </AlertDialogDescription>
                                    </AlertDialogHeader>
                                    <AlertDialogFooter>
                                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                                        <AlertDialogAction onClick={() => delRestaurant.mutate()} disabled={!id || Number.isNaN(id)}>Delete</AlertDialogAction>
                                    </AlertDialogFooter>
                                </AlertDialogContent>
                            </AlertDialog>
                        </div>
                    </div>
                </CardHeader>
            </Card>

            <Card>
                <CardHeader>
                    <div className="flex items-center gap-2">
                        <UtensilsCrossed className="h-5 w-5" />
                        <CardTitle>Menu Items</CardTitle>
                    </div>
                </CardHeader>
                <CardContent>
                    {menuItems.length === 0 ? (
                        <p className="text-muted-foreground text-center py-8">No menu items yet</p>
                    ) : (
                        <div className="space-y-4">
                            {menuItems.map((m: any, idx: number) => (
                                <div key={m.id}>
                                    {idx > 0 && <Separator className="my-4" />}
                                    <div className="flex justify-between items-start">
                                        <div>
                                            <h4 className="font-semibold">{m.name}</h4>
                                            {m.description && <p className="text-sm text-muted-foreground mt-1">{m.description}</p>}
                                        </div>
                                        <Badge variant="secondary">£{m.price}</Badge>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </CardContent>
            </Card>
        </div>
    )
}
