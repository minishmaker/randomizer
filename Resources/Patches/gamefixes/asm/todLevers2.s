.thumb
mov	r0,#0x34
ldr	r3,=#0x807C5F4
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	end

mov	r0,#0x35
ldr	r3,=#0x807C5F4
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	end

mov	r0,#0x36
ldr	r3,=#0x807C5F4
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	end

ldr	r3,=#0x8059FB7
bx	r3

end:
ldr	r3,=#0x8059FD5
bx	r3
