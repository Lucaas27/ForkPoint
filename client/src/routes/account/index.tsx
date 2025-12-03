import { createFileRoute, Link } from '@tanstack/react-router'
import { useMyRestaurants } from '../../features/account/queries'
import { useUpdateMe } from '../../features/account/mutations'
import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card'
import { Input } from '../../components/ui/input'
import { Label } from '../../components/ui/label'
import { Button } from '../../components/ui/button'
import { Separator } from '../../components/ui/separator'
import { Badge } from '../../components/ui/badge'
import { User, Store } from 'lucide-react'

export const Route = createFileRoute('/account/')({
    component: Account,
})

function Account() {
    const [name, setName] = useState('')
    const mUpdate = useUpdateMe()
    const { data } = useMyRestaurants()
    const restaurants = (data as any)?.restaurants ?? []

    return (
        <div className="max-w-2xl mx-auto space-y-6">
            <Card>
                <CardHeader>
                    <div className="flex items-center gap-2">
                        <User className="h-5 w-5" />
                        <CardTitle className="text-2xl">My Account</CardTitle>
                    </div>
                    <CardDescription>Manage your profile information</CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                    <div className="space-y-2">
                        <Label htmlFor="name">Display Name</Label>
                        <Input
                            id="name"
                            placeholder="Enter your name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />
                    </div>

                    <Button
                        onClick={() => mUpdate.mutate({ name })}
                        disabled={mUpdate.isPending}
                    >
                        {mUpdate.isPending ? 'Updating...' : 'Update Profile'}
                    </Button>
                </CardContent>
            </Card>

            <Card>
                <CardHeader>
                    <div className="flex items-center gap-2">
                        <Store className="h-5 w-5" />
                        <CardTitle>My Restaurants</CardTitle>
                    </div>
                    <CardDescription>
                        {restaurants.length === 0 ? 'You have no restaurants yet' : `Managing ${restaurants.length} restaurant${restaurants.length !== 1 ? 's' : ''}`}
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    {restaurants.length === 0 ? (
                        <div className="text-center py-8">
                            <p className="text-muted-foreground mb-4">Create your first restaurant to get started</p>
                            <Link to="/restaurants/create">
                                <Button>Create Restaurant</Button>
                            </Link>
                        </div>
                    ) : (
                        <div className="space-y-3">
                            {restaurants.map((r: any, idx: number) => (
                                <div key={r.id}>
                                    {idx > 0 && <Separator className="my-3" />}
                                    <div className="flex justify-between items-center">
                                        <div>
                                            <h4 className="font-semibold">{r.name}</h4>
                                            {r.category && <Badge variant="secondary" className="mt-1">{r.category}</Badge>}
                                        </div>
                                        <Link to="/restaurants/$id" params={{ id: r.id }}>
                                            <Button variant="outline" size="sm">View</Button>
                                        </Link>
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
