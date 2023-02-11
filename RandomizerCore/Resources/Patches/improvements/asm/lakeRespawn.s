.thumb
push	{r0}
@check if shallow water, if not then go vanilla
bl	getTile
cmp	r0,#0x3A
bne	vanilla

@check if minish or not
pop	{r0}
ldr	r0,=#0x3001160
ldrb	r0,[r0,#0x0C]
cmp	r0,#9
beq	small

big:
mov	r0,#0x20
b	end

small:
mov	r0,#0x00
b	end

vanilla:
pop	{r0}
ldr	r3,=#0x8000334
mov	lr,r3
.short	0xF800

end:
mov	r4,r0
mov	r0,r8
and	r4,r0
ldr	r3,=#0x807A3A6
mov	lr,r3
.short	0xF800

getTile:
ldr	r3,=#0x30057D4
bx	r3
