.thumb
mov	r4,#1
neg	r4,r4
push	{r0-r3}
@check if we are in the map screen
ldr	r0,=#0x20344A4
ldrb	r1,[r0]
cmp	r1,#6
bhi	notMap
cmp	r1,#4
blo	notMap

map:
@check if we are in a dungeon
ldr	r0,=#0x2033A90
ldrb	r2,[r0]
mov	r3,#8
and	r2,r3
cmp	r2,#0
beq	notMap
@check if select is pressed
ldr	r0,=#0x3000FF0
ldrh	r2,[r0,#2]
ldrh	r3,[r0,#4]
orr	r2,r3
mov	r3,#4
and	r2,r3
cmp	r2,#0
beq	notMap
@get the new value
ldr	r0,=#0x20344A4
mov	r2,#4
cmp	r1,#5
beq	world
mov	r2,#5
world:
strb	r2,[r0]
@and play sound
mov	r0,#0x65
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800
pop	{r0-r3}
pop	{r4-r5,pc}

notMap:
pop	{r0-r3}
ldr	r0,=#0x3000FF0
ldrh	r1,[r0,#2]
ldr	r3,=#0x80A47ED
bx	r3
