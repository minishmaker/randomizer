.thumb

ldr	r3, routine
cmp	r3, #0
beq	beam
push	{r0-r2}
mov	lr, r3
.short	0xF800
pop	{r0-r2}
cmp	r3, #0
beq	beam

vanilla:
add	r0, #0xAA
ldrb	r0, [r0]
cmp	r0, #0x08
bhi	nobeam

beam:
ldr	r3, =#0x807AA88
mov	lr, r3
.short 0xF800

nobeam:
ldr	r3, =#0x807AA90
mov	lr, r3
.short 0xF800

.align
.ltorg
routine: