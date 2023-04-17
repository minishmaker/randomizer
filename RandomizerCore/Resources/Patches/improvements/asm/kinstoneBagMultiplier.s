.thumb
@get multiplier
mov	r0,r4
sub	r0,#0x65
ldr	r3,multiplierTable
ldrb	r0,[r3,r0]
cmp	r0,#0
beq	skip

@add kinstone(s) to bag
push	{r0}
mov	r3,#0x8C
lsl	r3,r3,#1
add	r0,r2,r3
add	r0,r1,r0
strb	r4,[r0]
add	r3,#0x13
add	r0,r2,r3
add	r1,r0
ldrb	r0,[r1]
pop	{r3}
add	r0,r3
ldr	r3,=#0x801E7C1
bx	r3

skip:
ldr	r3,=#0x801E7C9
bx	r3

.align
.ltorg
multiplierTable:
