.thumb
push	{r4,r5,lr}
mov	r4,r0
mov	r1,r4
add	r1,#0x58
push	{r1-r7}

@check if trap
ldrb	r0,[r4,#0x0A]
cmp	r0,#0x1B
bne	end

@get the new icon to use
mov	r0,r4
ldr	r3,trapGetIcon
mov	lr,r3
.short	0xF800

end:
pop	{r1-r7}
ldrb	r3,=#0x808069F
bx	r3
.align
.ltorg
trapGetIcon:
