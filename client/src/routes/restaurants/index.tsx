import { createFileRoute, Link } from '@tanstack/react-router'
import { useRestaurants } from '../../features/restaurants/queries'
import { Card, CardDescription, CardHeader, CardTitle } from '../../components/ui/card'
import { Button } from '../../components/ui/button'
import { Badge } from '../../components/ui/badge'
import { Skeleton } from '../../components/ui/skeleton'
import { Plus, ChevronRight } from 'lucide-react'

export const Route = createFileRoute('/restaurants/')({
    component: Restaurants,
})

function Restaurants() {
    const { data, isLoading, error } = useRestaurants()

    if (isLoading) {
        return (
            <div className="space-y-4">
                <Skeleton className="h-12 w-full" />
                <Skeleton className="h-32 w-full" />
                <Skeleton className="h-32 w-full" />
            </div>
        )
    }

    if (error) {
        return (
            <Card className="border-destructive">
                <CardHeader>
                    <CardTitle className="text-destructive">Error</CardTitle>
                    <CardDescription>Failed to load restaurants</CardDescription>
                </CardHeader>
            </Card>
        )
    }

    const restaurants = (data as any)?.restaurants ?? (data as any)?.items ?? []
    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h2 className="text-3xl font-bold tracking-tight">Restaurants</h2>
                    <p className="text-muted-foreground">Browse all available restaurants</p>
                </div>
                <Link to="/restaurants/create">
                    <Button>
                        <Plus className="h-4 w-4 mr-2" />
                        Create Restaurant
                    </Button>
                </Link>
            </div>
            <div className="grid gap-4">
                {restaurants.map((r: any) => (
                    <Card key={r.id} className="hover:shadow-lg transition-all">
                        <CardHeader>
                            <div className="flex items-start justify-between">
                                <div className="space-y-1">
                                    <CardTitle>{r.name}</CardTitle>
                                    <CardDescription>{r.description}</CardDescription>
                                    {r.category && <Badge variant="secondary">{r.category}</Badge>}
                                </div>
                                <Link to="/restaurants/$id" params={{ id: r.id }}>
                                    <Button variant="ghost" size="sm">
                                        View
                                        <ChevronRight className="h-4 w-4 ml-1" />
                                    </Button>
                                </Link>
                            </div>
                        </CardHeader>
                    </Card>
                ))}
            </div>
        </div>
    );
}
