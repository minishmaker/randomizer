.thumb
push	{r4,lr}
mov	r4,r0

mov	r0,#0x86
ldrh	r0,[r4,r0]
cmp	r0,#0
bne	end

ldr	r3,=#0x805DE94
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	end

return:
ldr	r3,=#0x8080EA1
bx	r3

end:
ldr	r3,=#0x8080ECF
bx	r3
