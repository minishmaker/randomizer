.thumb
ldr	r0, =#0x3003DF0
mov	r2, #0xF1
add	r0, #0x8

@ store in r6 whether we cannot safely reload fusions for fusers offering the just completed fusion
mov	r6, #1
cmp	r4, #0x21 @ Percy (leaves house after this fusion)
beq	loop
cmp	r4, #0x25 @ 2nd Goron
beq	loop
cmp	r4, #0x26 @ 4th Goron
beq	loop
cmp	r4, #0x29 @ 1st Goron
beq	loop
cmp	r4, #0x2A @ 3rd Goron
beq	loop
cmp	r4, #0x2B @ 5th Goron
beq	loop
cmp	r4, #0x2F @ 6th Goron
beq	loop

ldr	r3, =#0x2002B35
ldrb	r3, [r3]
and	r3, r6
cmp	r3, #1
beq	hasMagicBoomerang

cmp	r4, #0x59 @ first Tingle fusion
beq	loop
cmp	r4, #0x5A @ first David Jr. fusion
beq	loop

hasMagicBoomerang:
mov	r6, #0

loop:
ldrb	r3, [r0, #3]
cmp	r4, r3
bne	continue

@ mark fuser as having its current fusion completed (same as vanilla)
strb	r2, [r0, #3]

@ check if we can safely reload the available fusion
cmp	r6, #0
bne	continue

push	{r0-r2}
mov	r5, r0
ldr	r0, [r0, #8]
cmp	r0, #0
beq	noEntity

@ GetFusionToOffer
ldr	r3, =#0x801E99C
mov	lr, r3
.short	0xF800
strb	r0, [r5, #3]

noEntity:
pop	{r0-r2}

continue:
add	r0, #12
add	r1, #1
cmp	r1, #0x1F
bls	loop

pop	{r4-r6, pc}
