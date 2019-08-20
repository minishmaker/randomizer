.thumb
push	{r4-r7,lr}
mov	r4,r0	@item

@0x48 -> 0x73
@0x48 -> 0x74
@0x48 -> 0x4D -> 0x75

@check what chain this item is from
cmp	r4,#0x73
beq	fastSpin
cmp	r4,#0x74
beq	fastSplit
cmp	r4,#0x75
beq	greatSpin
cmp	r4,#0x4D
beq	greatSpin
cmp	r4,#0x48
beq	normalSpin
b	end

normalSpin:
bl	hasSpin
cmp	r0,#0
beq	end
b	findSpot

giveSpin:
mov	r0,#0x48
b	end

giveGreat:
mov	r0,#0x4D
b	end

giveFastSpin:
mov	r0,#0x73
b	end

giveFastSplit:
mov	r0,#0x74
b	end

giveLongerGreat:
mov	r0,#0x75
b	end

fastSpin:
bl	hasSpin
cmp	r0,#0
beq	giveSpin
mov	r0,#0x73
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	giveFastSpin
b	findSpot

fastSplit:
bl	hasSpin
cmp	r0,#0
beq	giveSpin
mov	r0,#0x74
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	giveFastSplit
b	findSpot

greatSpin:
bl	hasSpin
cmp	r0,#0
beq	giveSpin
mov	r0,#0x4D
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	giveGreat
mov	r0,#0x75
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	giveLongerGreat
b	findSpot

@since the branch failed, we will look for a free spot in any branch
findSpot:
bl	hasSpin
cmp	r0,#0
beq	giveSpin
mov	r0,#0x73
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	giveFastSpin
mov	r0,#0x74
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	giveFastSplit
mov	r0,#0x4D
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	giveGreat
mov	r0,#0x75
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	giveLongerGreat
b	giveNothing

giveNothing:
mov	r0,#0
b	end

end:
pop	{r4-r7,pc}

hasSpin:
ldr	r0,=#0x2002B44
ldrb	r0,[r0]
mov	r1,#1
and	r0,r1
bx	lr
